char incoming;
char str[4];
int i=0;
void setup() {
Serial.begin(9600);
}
void loop() {
while (Serial.available()) {
incoming = Serial.read();
if (incoming < '0' || incoming > '9') break;
str[i] = incoming;
i++;
// Serial.println(i);
}
if (i > 2) //на данные отводим три знака
{
str[i] = 0;
Serial.println(atoi(str));
i=0;
}
}
