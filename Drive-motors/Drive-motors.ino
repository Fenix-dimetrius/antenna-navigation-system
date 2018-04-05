/*
  Tilt compensated HMC5883L + MPU6050 (GY-86 / GY-87). Output for HMC5883L_compensation_processing.pde
  Read more: http://www.jarzebski.pl/arduino/czujniki-i-sensory/3-osiowy-magnetometr-hmc5883l.html
  GIT: https://github.com/jarzebski/Arduino-HMC5883L
  Web: http://www.jarzebski.pl
  (c) 2014 by Korneliusz Jarzebski
*/

#include <Wire.h>
#include <HMC5883L.h>
#include <MPU6050.h>

HMC5883L compass;
MPU6050 mpu;
int IN3 = 5; // Input3 подключен к выводу 5
int IN4 = 4;
int ENB = 3; // разрешающий вывод подключен к выводу 3
int IN1 = 7; // Input1 подключен к выводу 7
int IN2 = 6;
int ENA = 9; // разрешающий вывод подключен к выводу 8
int interval = 0;
//int IN1 = 7; // Input1 подключен к выводу 5
//int IN2 = 6;
//int IN3 = 4;
//int IN4 = 5;
//int ENA = 9;
//int ENB = 3;
int driveRotKey = 2; // показывает, вращаются ли двигатели. три состояния:
					// 0 - El; 1 - Az; 2 - двигатели не вращаются
int valBuf = 0;
float val = 0;
float heading1; // значение компаса
float heading2;
//принимаемые данные
int driveCh; // выбор двигателя. 1 - азимутальный; 0 - по углу места.
int driveDir; // указывает направление вращения: "1" - по часовой; "0" - против часовой 
int driveSp;// скорость вращения двигателя

void setup()
{
	Serial.begin(115200);

	// Initialize MPU6050
	while(!mpu.begin(MPU6050_SCALE_2000DPS, MPU6050_RANGE_2G))
	{
		delay(500);
	}

	// Enable bypass mode
	mpu.setI2CMasterModeEnabled(false);
	mpu.setI2CBypassEnabled(true) ;
	mpu.setSleepEnabled(false);

	// Initialize Initialize HMC5883L
	while (!compass.begin())
	{
		delay(500);
	}

	// Set measurement range
	compass.setRange(HMC5883L_RANGE_1_3GA);

	// Set measurement mode
	compass.setMeasurementMode(HMC5883L_CONTINOUS);

	// Set data rate
	compass.setDataRate(HMC5883L_DATARATE_30HZ);

	// Set number of samples averaged
	compass.setSamples(HMC5883L_SAMPLES_8);

	// Set calibration offset. See HMC5883L_calibration.ino
	compass.setOffset(0, 0);
}

float calibrated_values[3];
//transformation(float uncalibrated_values[3]) is the function of the magnetometer data correction
//uncalibrated_values[3] is the array of the non calibrated magnetometer data
//uncalibrated_values[3]: [0]=Xnc, [1]=Ync, [2]=Znc
void transformation(float uncalibrated_values[3])
{
	//calibration_matrix[3][3] is the transformation matrix
	//replace M11, M12,..,M33 with your transformation matrix data
	double calibration_matrix[3][3] = {
		{0.000929, 0.000033,0.000001},
		{0.000033, 0.000923, -0.000033},
		{0.000001, -0.000033, 0.001086}
	};
	//bias[3] is the bias
	//replace Bx, By, Bz with your bias data
	double bias[] = {
		16.572225,
		-203.935684,
		-8.637929
	};
	
	//calculation
	for (int i=0; i<3; ++i) uncalibrated_values[i] = uncalibrated_values[i] - bias[i];
	float result[3] = {0, 0, 0};
	for (int i=0; i<3; ++i)
	for (int j=0; j<3; ++j)
	result[i] += calibration_matrix[i][j] * uncalibrated_values[j];
	for (int i=0; i<3; ++i) calibrated_values[i] = result[i];
}

// No tilt compensation
float noTiltCompensate(Vector mag)
{
	float heading = atan2(mag.YAxis, mag.XAxis);
	return heading;
}

// Tilt compensation
float tiltCompensate(Vector mag, Vector normAccel)
{
	// Pitch & Roll
	
	float roll;
	float pitch;
	
	roll = asin(normAccel.YAxis);
	pitch = asin(-normAccel.XAxis);

	if (roll > 0.78 || roll < -0.78 || pitch > 0.78 || pitch < -0.78)
	{
		return -1000;
	}
	
	// Some of these are used twice, so rather than computing them twice in the algorithem we precompute them before hand.
	float cosRoll = cos(roll);
	float sinRoll = sin(roll);
	float cosPitch = cos(pitch);
	float sinPitch = sin(pitch);
	
	// Tilt compensation
	float Xh = mag.XAxis * cosPitch + mag.ZAxis * sinPitch;
	float Yh = mag.XAxis * sinRoll * sinPitch + mag.YAxis * cosRoll - mag.ZAxis * sinRoll * cosPitch;
	
	float heading = atan2(Yh, Xh);
	
	return heading;
}

// Correct angle
float correctAngle(float heading)
{
	if (heading < 0) { heading += 2 * PI; }
	if (heading > 2 * PI) { heading -= 2 * PI; }

	return heading;
}

// двигатель вращения по азимуту
void driveAzMoveForward () //поворот по часовой
{
	driveRotKey = 1; // показываем, что вращается двигатель по Az
	//защита
	analogWrite(ENB,0);
	digitalWrite (IN3, LOW);
	digitalWrite (IN4, LOW);
	analogWrite(ENA,0);
	// На пару выводов "IN" поданы разноименные сигналы, мотор готов к вращаению
	digitalWrite (IN1, HIGH);
	digitalWrite (IN2, LOW);
	analogWrite(ENA,interval);

}

void driveAzMoveBack () // поворот против часовой
{
	driveRotKey = 1; // показываем, что вращается двигатель по Az
	//защита
	analogWrite(ENB,0);
	digitalWrite (IN3, LOW);
	digitalWrite (IN4, LOW);
	analogWrite(ENA,0);
	// На пару выводов "IN" поданы разноименные сигналы, мотор готов к вращаению
	digitalWrite (IN1, LOW);
	digitalWrite (IN2, HIGH);
	analogWrite(ENA,interval);
	
	
	
}

// двигатель вращения по углу места
void driveElMoveForward () //поворот по часовой
{
	driveRotKey = 0; // показываем, что вращается двигатель по El
	//защита
	analogWrite(ENA,0);
	digitalWrite (IN1, LOW);
	digitalWrite (IN2, LOW);
	analogWrite(ENB,0);
	// На пару выводов "IN" поданы разноименные сигналы, мотор готов к вращаению
	digitalWrite (IN3, HIGH);
	digitalWrite (IN4, LOW);
	analogWrite(ENB,interval);
	
}

void driveElMoveBack () // поворот против часовой
{
	driveRotKey = 0; // показываем, что вращается двигатель по El
	analogWrite(ENB,0);
	//защита
	analogWrite(ENA,0);
	digitalWrite (IN1, LOW);
	digitalWrite (IN2, LOW);
	// На пару выводов "IN" поданы разноименные сигналы, мотор готов к вращаению
	digitalWrite (IN3, LOW);
	digitalWrite (IN4, HIGH);
	analogWrite(ENB,interval);
	
	
}

void sensorVal()
{
	 // Pitch, Roll and Yaw values
	 float pitchG = 0;
	 float rollG = 0;
	 float yawG = 0;
	 // Timers
	 unsigned long timer = 0;
	 float timeStep = 0.01;
	 timer = millis();
	 float values_from_magnetometer[3];
	 // Read normalized values Gyro
	 Vector normGyro = mpu.readNormalizeGyro();
	 // Read vectors
	 Vector mag = compass.readNormalize();
	 //Vector mag = compass.readRaw();
	 Vector acc = mpu.readNormalizeAccel();
	 // Read normalized Accel
	 Vector normAccel = mpu.readNormalizeAccel();
	 // Read Temperature
	 float temp = mpu.readTemperature();
	 values_from_magnetometer[0] = mag.XAxis;
	 values_from_magnetometer[1] = mag.YAxis;
	 values_from_magnetometer[2] = mag.ZAxis;
	 transformation(values_from_magnetometer);
	 mag.XAxis = calibrated_values[0];
	 mag.YAxis = calibrated_values[1];
	 mag.ZAxis = calibrated_values[2];

	 // Calculate Pitch, Roll and Yaw
	 pitchG = pitchG + normGyro.YAxis * timeStep;
	 rollG = rollG + normGyro.XAxis * timeStep;
	 yawG = yawG + normGyro.ZAxis * timeStep;

	 // Calculate Pitch & Roll
	 float pitch = -(atan2(normAccel.XAxis, sqrt(normAccel.YAxis*normAccel.YAxis + normAccel.ZAxis*normAccel.ZAxis))*180.0)/M_PI;
	 float roll = (atan2(normAccel.YAxis, normAccel.ZAxis)*180.0)/M_PI;

	 // Calculate headings
	 heading1 = noTiltCompensate(mag);
	 heading2 = tiltCompensate(mag, acc);
	 
	 if (heading2 == -1000)
	 {
		 heading2 = heading1;
	 }

	 // Set declination angle on your location and fix heading
	 // You can find your declination on: http://magnetic-declination.com/
	 // (+) Positive or (-) for negative
	 // For Bytom / Poland declination angle is 4'26E (positive)
	 // Formula: (deg + (min / 60.0)) / (180 / M_PI);
	 float declinationAngle = (6.0 + (31.0 / 60.0)) / (180 / M_PI);
	 heading1 += declinationAngle;
	 heading2 += declinationAngle;
	 
	 // Correct for heading < 0deg and heading > 360deg
	 heading1 = correctAngle(heading1);
	 heading2 = correctAngle(heading2);

	 // Convert to degrees
	 heading1 = heading1 * 180/M_PI;
	 heading2 = heading2 * 180/M_PI;


	 Serial.print(temp);
	 Serial.print("|");
	 Serial.print(heading1);
	 Serial.print("|");
	 Serial.print(pitch);
	 Serial.print("|");
	 Serial.print(roll);
	 Serial.print("|");
	 Serial.println(driveRotKey);
}
int s1, s2, s3;
void controlDate()
{
	int i=0;
	
 char buffer[100];
 
 //если есть данные - читаем
 if(Serial.available())
 {
	 delay(100);
	 
	 //загоняем прочитанное в буфер
	 while( Serial.available() && i< 99)
	 {
		 buffer[i++] = Serial.read();
	 }
	 //закрываем массив
	 buffer[i++]='\0';
 }
 
 //если буфер наполнен
 if(i>0)
 {
	 driveCh=atoi(strtok(buffer," "));
	 driveDir=atoi(strtok(NULL," "));
	 driveSp=atoi(strtok(NULL," "));
	 
	 	//driveCh=atoi(strtok(buffer," "));
	 	//driveDir=atoi(strtok(NULL," "));
	 	//driveSp=atoi(strtok(NULL," "));
	 //s1=atoi(strtok(buffer," "));
	 //s2=atoi(strtok(NULL," "));
	 //s3=atoi(strtok(NULL," "));
	 //
 }
  delay (10); 
	
	
}

void loop()
{
	
	sensorVal(); //вычисление азимута и угла места с последующей отправкой в COM-port.
	controlDate();

	int i1 = 0;
	interval = driveSp;
	if (driveSp == 0)
		{			
			digitalWrite (IN1, LOW);
			digitalWrite (IN2, LOW);
			digitalWrite (IN3, LOW);
			digitalWrite (IN4, LOW);
			analogWrite(ENA,0);
			analogWrite(ENB,0);
			driveRotKey = 2;
			
		}
		
		else
		{
			
			if (driveCh == 0)
				{
					if (driveDir == 0)
					{
						driveElMoveBack ();
					}
					if (driveDir == 1)
					{
						driveElMoveForward ();
					}
					
				}
				
			if (driveCh == 1)
			{
				if (driveDir == 0)
				{
					driveAzMoveBack ();
				}
				if (driveDir == 1)
				{
					driveAzMoveForward ();
				}
				
			}			
				
			
		}
		
	
		
		//if (s3 == 0)
		//{			
			//digitalWrite (IN1, LOW);
			//digitalWrite (IN2, LOW);
			//digitalWrite (IN3, LOW);
			//digitalWrite (IN4, LOW);
		//}
		//
		//else
		//{
			//interval = s3;
			//if (s1 == 0)
				//{
					//if (s2 == 0)
					//{
						//driveElMoveBack ();
					//}
					//if (s2 == 1)
					//{
						//driveElMoveForward ();
					//}
					//
				//}
				//
			//if (s1 == 1)
			//{
				//if (s2 == 0)
				//{
					//driveAzMoveBack ();
				//}
				//if (s2 == 1)
				//{
					//driveAzMoveForward ();
				//}
				//
			//}			
				//
			//
		//}
		
		//delay(30);
	//driveAzMoveForward ();
	//driveElMoveBack ();
}

