#include "Wire.h"
#include "HMC5883L.h"
#include "math.h"
int IN3 = 5; // Input3 подключен к выводу 5
int IN4 = 4;
int ENB = 3;
int interval = 0;
int valBuf = 0;
float val = 0;
float heading;
HMC5883L compass;

void setup()
{
	Serial.begin(9600);
	Wire.begin();
	pinMode (ENB, OUTPUT);
	pinMode (IN3, OUTPUT);
	pinMode (IN4, OUTPUT);
	compass = HMC5883L();  // создаем экземпляр HMC5883L библиотеки
	setupHMC5883L();       // инициализация HMC5883L
	
	
}


void DriveMoveForward () //поворот по часовой
{
	analogWrite(ENB,0);
	// На пару выводов "IN" поданы разноименные сигналы, мотор готов к вращаению
	digitalWrite (IN3, HIGH);
	digitalWrite (IN4, LOW);
	analogWrite(ENB,interval);
}

void DriveMoveBack () // поворот против часовой
{
	analogWrite(ENB,0);
	// На пару выводов "IN" поданы разноименные сигналы, мотор готов к вращаению
	digitalWrite (IN3, LOW);
	digitalWrite (IN4, HIGH);
	analogWrite(ENB,interval);
	
}

void loop()
{
	bool key = false;
//	val = heading;
	heading = getHeading(); // значение угла на север.
	Serial.print(heading);
	Serial.print("		");
	Serial.print(val);
	Serial.print("		");
	Serial.println(interval);
	char buffer[100];
	int i=0;
	//если есть данные - читаем
	if(Serial.available()){
		delay(100);
		
		//загоняем прочитанное в буфер
		while( Serial.available() && i< 99) {
			buffer[i++] = Serial.read();
		}
		//закрываем массив
		buffer[i++]='\0';
		val = atoi(strtok(buffer," "));
		key = true;
		
	}
	else 
	{  
		//
		//
			//val = (heading);
			////key =true;
		//}
		delay(100);
	}
	
	int n1 = 180, n = 0; // шаги скорости двигателей
	//if (val == 500) key=true; else key = false; // проверка на внешнюю остановку двигателя
	
		
		if (abs(heading - val) < 1 || val == 0)
		{
			//key = false;
			digitalWrite (IN3, LOW);
			digitalWrite (IN4, LOW);
		}		
		
		else
		{
			if (val > 0 && val <=180 && heading > 0 && heading <=180)
			{
				if (heading < val)
					{
						if (abs(heading - val) >= 10 )
						{
							interval = 255;
							DriveMoveBack (); // движемся против часовой
						}
						
						if (abs(heading - val) < 10 )
						{
							interval = int(155*(1-(10 - abs(heading - val))/10.0)+100);
							DriveMoveBack (); // движемся против часовой
						}					
						}
						else
						{
							if (abs(heading - val) >= 10 )
							{
								interval = 255;
								DriveMoveForward (); // движемся против часовой
							}
							if (abs(heading - val) < 10 )
							{
							interval = int(155*(1-(10 - abs(heading - val))/10.0)+100);
							DriveMoveForward (); // движемся против часовой
							}
				
						}
				
			}
			
			if (val > 180 && val <=360 && heading > 180 && heading <=360)
			{
				
			}
			
			if (val > 0 && val <=180 && heading > 270 && heading <=360)
			{
				if (abs(heading - val) > 180)
				{
					
				}
				else
				{
					
				}
				
			}
			
			if (val > 180 && val <=360 && heading > 0 && heading <=180)
			{
				if (abs(heading - val) > 180)
				{
					
				}
				else
				{
					
				}
			}

		}
	

}

void setupHMC5883L(){
	// инициализация HMC5883L, и проверка наличия ошибок
	int error;
	error = compass.SetScale(0.88); // чувствительность датчика из диапазона: 0.88, 1.3, 1.9, 2.5, 4.0, 4.7, 5.6, 8.1
	if(error != 0) Serial.println(compass.GetErrorText(error)); // если ошибка, то выводим ее
	
	error = compass.SetMeasurementMode(Measurement_Continuous); // установка режима измерений как Continuous (продолжительный)
	if(error != 0) Serial.println(compass.GetErrorText(error)); // если ошибка, то выводим ее
}

float getHeading(){
	// считываем данные с HMC5883L и рассчитываем  направление
	MagnetometerScaled scaled = compass.ReadScaledAxis(); // получаем масштабированные элементы с датчика
	float heading = atan2(scaled.YAxis, scaled.XAxis);    // высчитываем направление
	
	// корректируем значения с учетом знаков
	if(heading < 0) heading += 2*PI;
	if(heading > 2*PI) heading -= 2*PI;
	
	if (heading * RAD_TO_DEG >= 45) heading = heading * RAD_TO_DEG - 45; // 0..315
	if (heading * RAD_TO_DEG < 45) heading = 360 - 45 - heading * RAD_TO_DEG; // 360..315
	
	return heading; // переводим радианы в градусы
}


