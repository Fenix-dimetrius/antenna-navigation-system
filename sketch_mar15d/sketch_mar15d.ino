#include <string.h>
int redPin = 11;
int greenPin = 10;
int bluePin = 9;
char buffer[11];
int s1, s2, s3;
 
void setup()
{
  Serial.begin(9600);
  pinMode(redPin, OUTPUT);
  pinMode(greenPin, OUTPUT);
  pinMode(bluePin, OUTPUT);
}
 
void loop()
{
int i=0;
  char buffer[100];
 
  //если есть данные - читаем
  if(Serial.available()){
    delay(100);
 
    //загоняем прочитанное в буфер
    while( Serial.available() && i< 99) {
      buffer[i++] = Serial.read();
    }
    //закрываем массив
    buffer[i++]='\0';
  }
 
  //если буфер наполнен
  if(i>0){
              s1=atoi(strtok(buffer," "));
              s2=atoi(strtok(NULL," "));
              s3=atoi(strtok(NULL," "));   
          }
  setColor(s1, s2, s3);
  delay (10); 
}
 
void setColor(int red, int green, int blue)
{
  analogWrite(redPin, red);
  analogWrite(greenPin, green);
  analogWrite(bluePin, blue);
}
