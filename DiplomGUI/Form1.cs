using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;
//using Tao.OpenGl;
//using Tao.FreeGlut;
//using Tao.Platform.Windows; 

namespace DiplomGUI
{
    public partial class Form1 : Form
    {
        static bool key = false;  
        public Form1()
        {
            InitializeComponent();
           
        }
        float angle = 0, pitch = 0;
        //Image image1 = Image.FromFile("E://compassPlateBlack.png");
        //Image image12 = Image.FromFile("E://compassRing.png");
        //Image image2 = Image.FromFile("E://Ant2.png"); 

        Image image1 = Image.FromFile("./compassPlateBlack.png");
        Image image12 = Image.FromFile("./compassRing.png");
        Image image2 = Image.FromFile(@"./Ant2.png"); 
        private void Form1_Load(object sender, EventArgs e)
        {



           // timer1.Interval =10;
           // timer1.Start();
            // получаем список доступных портов
            string[] ports = SerialPort.GetPortNames();
            label21.Text = Convert.ToString(trackBar1.Value);
            // выводим список портов
            for (int i = 0; i < ports.Length; i++)
            {
                comboBox1.Items.Add(ports[i].ToString());
                             
            }
           
       // рисование компасов
           
            pictureBox1.Paint += new PaintEventHandler(pictureBox1_2_Paint);
            pictureBox1.Paint += new PaintEventHandler(pictureBox1_Paint);
            pictureBox2.Paint += new PaintEventHandler(pictureBox2_Paint);
        }



        void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            //e.Graphics.TranslateTransform(pictureBox1.Width / 2, pictureBox1.Height / 2);
            // e.Graphics.RotateTransform(0);
            // e.Graphics.TranslateTransform(0 - image1.Width / 2,0 - image1.Height / 2);
            // e.Graphics.DrawImage(image2, pictureBox1.Width /14, -1+pictureBox1.Height / 90);// 48,28);
            e.Graphics.TranslateTransform(pictureBox1.Width/2-44, pictureBox1.Height/2-12);
            //Rotate.        
            e.Graphics.RotateTransform(angle);
            //Move image back.

            e.Graphics.TranslateTransform(-33 - image1.Width / 2,-33- image1.Height / 2);
            e.Graphics.DrawImage(image1, 0, 0);
            

        }
        void pictureBox1_2_Paint(object sender, PaintEventArgs e)
        {
            // e.Graphics.TranslateTransform(0, 0);

            e.Graphics.TranslateTransform(pictureBox1.Width / 2, pictureBox1.Height / 2);
            //Rotate.        
            e.Graphics.RotateTransform(0);
            //Move image back.
            e.Graphics.TranslateTransform(-33 - image12.Width / 2, -33 - image12.Height / 2);
            // e.Graphics.TranslateTransform(-pictureBox2.Width / 2, -pictureBox2.Height / 2);
            e.Graphics.DrawImage(image12, 2, 2);
            // e.Graphics.TranslateTransform(33 + image1.Width / 2, 33 + image1.Height / 2);

        }


        void pictureBox2_Paint(object sender, PaintEventArgs e)
        {
            //e.Graphics.TranslateTransform(pictureBox1.Width / 2, pictureBox1.Height / 2);
            // e.Graphics.RotateTransform(0);
            // e.Graphics.TranslateTransform(0 - image1.Width / 2,0 - image1.Height / 2);
            // e.Graphics.DrawImage(image2, pictureBox1.Width /14, -1+pictureBox1.Height / 90);// 48,28);
            e.Graphics.TranslateTransform(pictureBox1.Width / 2 , pictureBox1.Height / 2 );
            //Rotate.        
            e.Graphics.RotateTransform(pitch);
            //Move image back.

            e.Graphics.TranslateTransform( -15- image2.Width / 2 , -45 - image2.Height / 2);
            e.Graphics.DrawImage(image2, 0, 0);



        }
        private void button1_Click(object sender, EventArgs e)
        {

         
            try
            {
                serialPort1.PortName = comboBox1.Text;
                serialPort1.Open();
                key = false;
                backgroundWorker1.RunWorkerAsync();
                backgroundWorker2.RunWorkerAsync();
                button3.Enabled=true;
                button5.Enabled = true;
                button9.Enabled = true;
                button11.Enabled = true;
                button4.Enabled = true;
                button10.Enabled = true;
                textBox1.Text = "Connect to com";
                // timer1.Start();

            }
            catch (Exception e1)
            {
                MessageBox.Show(e1.Message); //если ком порт не открылся, показываем почему.           
            }

            
        }


        
        void click1()
        {
            //textBox3.Text = _value;
        }
        
        private void button2_Click(object sender, EventArgs e)
        {
           
        }

        private void button3_Click(object sender, EventArgs e)
        {
           
        }

        private void button4_Click(object sender, EventArgs e)
        {
            serialPort1.WriteLine("1 1 " + trackBar1.Value);

            for (int i1 = 0; i1 <= 300; i1++)
            {
                //    serialPort1.WriteLine("0 1 " + trackBar1.Value);
                serialPort1.WriteLine("1 0 0");

            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
           
            serialPort1.WriteLine("1 0 " + trackBar1.Value);
          
            for (int i1 = 0; i1 <= 300; i1++)
            {
                //    serialPort1.WriteLine("0 1 " + trackBar1.Value);
                serialPort1.WriteLine("1 0 0");

            }
        }

        private void textBox1_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            // Determine whether the key entered is the F1 key. If it is, display Help.
            if (e.KeyCode == Keys.F1 && (e.Alt || e.Control || e.Shift))
            {
                // Display a pop-up Help topic to assist the user.
                Help.ShowPopup(textBox1, "Enter your name.", new Point(textBox1.Bottom, textBox1.Right));
            }
            else if (e.KeyCode == Keys.F2 && e.Modifiers == Keys.Alt)
            {
                // Display a pop-up Help topic to provide additional assistance to the user.
                Help.ShowPopup(textBox1, "Enter your first name followed by your last name. Middle name is optional.",
                    new Point(textBox1.Top, this.textBox1.Left));
            }
        }

    
     

   

        private void Form1_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        { 
         
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("Hello");
            button3.Enabled = false;
            button5.Enabled = false;
            button9.Enabled = false;
            button11.Enabled = false;
            button4.Enabled = false;
            button10.Enabled = false;
            key = true;
            try
            {
               
                serialPort1.Close();
                backgroundWorker1.CancelAsync();
                backgroundWorker1.Dispose();
                backgroundWorker2.CancelAsync();
                backgroundWorker2.Dispose();
                
            }
            catch (Exception e1)
            {
                MessageBox.Show(e1.Message); //показываем ошибку.           
            }

        }



        // Фильтр Калмана для акселерометра
        double xAcelOpt = 1;// прогнозируемое значение
       // double xKvSum = 0;
        int k1 = 0;
        double eOptKv = 0;
       void KalmanFiltr(double zAcel)
        {
           // double xOpt = 0;
            double sigmaPsi = 2;
            double sigmaEta = 15;
            double KaGain  = 0; // усиление Калмана
           
            //счетчик шагов для определения дисперсии.

           // if (k1 < n1)
           // {
           //     xKvSum += Math.Pow(zAcel, 2); 
           //     k1++;
               
             
           // }
           //else
           // { 
                k1 = 0; // обнуляем счетчик
               // sigmaEta = (xKvSum - (xKvSum / n1)) / (n1 - 1);// находим дисперсию сенсора.
              //  xKvSum = 0; //обнуляем сумму квадратов показаний сенсора за n шагов
                eOptKv = (Math.Pow(sigmaEta, 2)) * (eOptKv + Math.Pow(sigmaPsi, 2)) / (Math.Pow(sigmaEta, 2) + eOptKv + Math.Pow(sigmaPsi, 2)); // находим среднее значение квадрата ошибки
                KaGain = eOptKv / Math.Pow(sigmaEta, 2); // Вычисляем кофициент Калмана.
                xAcelOpt = KaGain * zAcel + (1 - KaGain) * xAcelOpt; // оптимальное значение.
           //}
               
            //return xAcelOpt[1];
        }


        //фильтр Калмана для магнитометра
       double xMagOpt = 1;// прогнозируемое значение
       //double xKvSumMag = 0;
       
       double eOptKvMag = 0;
       void KalmanFiltrMag(double zMag)
       {
           // double xOpt = 0;
           double sigmaPsi = 1;
           double sigmaEta = 5;
           double KaGain = 0; // усиление Калмана
          
           //счетчик шагов для определения дисперсии.

           // if (k1 < n1)
           // {
           //     xKvSum += Math.Pow(zAcel, 2); 
           //     k1++;


           // }
           //else
           // { 
          // k1 = 0; // обнуляем счетчик
           // sigmaEta = (xKvSum - (xKvSum / n1)) / (n1 - 1);// находим дисперсию сенсора.
          // xKvSumMag = 0; //обнуляем сумму квадратов показаний сенсора за n шагов
           eOptKvMag = (Math.Pow(sigmaEta, 2)) * (eOptKvMag + Math.Pow(sigmaPsi, 2)) / (Math.Pow(sigmaEta, 2) + eOptKvMag + Math.Pow(sigmaPsi, 2)); // находим среднее значение квадрата ошибки
           KaGain = eOptKvMag / Math.Pow(sigmaEta, 2); // Вычисляем кофициент Калмана.
           xMagOpt = KaGain * zMag + (1 - KaGain) * xMagOpt; // оптимальное значение.
           //}

           //label17.Invoke(new MethodInvoker(delegate() { label17.Text = Convert.ToString(KaGain); }));
          // label17.Invoke(new MethodInvoker(delegate() { label17.Text = Convert.ToString(xKvSumMag); }));
           //return xAcelOpt[1];
       }
       int driveRotKeyNow = 2;
       int driveRotKey = 2; // показывает, вращаются ли двигатели. три состояния:
       // 0 - El; 1 - Az; 2 - ниодин двигатель не вращается

       int rotSp = 0;
       int rotSpAz = 0;
       int rotSpAzOld = 0;
       int rotSpOld = 0;
       bool orientChan = false;
       double az = 0;
       double el = 0;
       bool calibrKey = true;
       bool calibrKeyAz = true;
       bool calibrOldKey = true;
       bool calibrOldKeyAz = true;
       double oldEl;
       double oldAz;
       bool stopKey = true;
       bool stopKeyAz = true;
       double delta = 0;
       double dSigmaFull = 0; // угловая разница
       double dSigmaEl = 0; // угловая разница
       double dSigmaAz = 0; // угловая разница
       double dSigmaOld = 0;
       double dSigmaOldAz = 0;
       double dSigmaOld2El = 0;
       double dSigmaOld2Az = 0;
       string moveDir = "0 "; //начальное вращение
       string moveDirAz = "0 "; //начальное вращение
       string mdir = "Az";
       int n2 = 0;
       double sigmaSample = 0;
       bool azOrientComp = false;
       bool elOrientComp = false;
       bool OrientComp = false;
       bool AzRot()
       {
           if (dSigmaAz < 1) // если разницамежду заданным и текущим менье 1 градуса, то останавливаемся.
           {

               if (stopKeyAz == true) // отправляем единожды пачку команд на остановку за все время пербывания в этом положении. 
               {
                   stopKeyAz = false;
                   oldAz = xMagOpt;
                   //n2 = 0;
                   for (int k1 = 0; k1 <= 300; k1++)
                   {

                       serialPort1.WriteLine("1 0 0"); // тормоз
                   }
                  
                   if (moveDirAz == "0 ") moveDirAz = "1 ";
                   else moveDirAz = "0 ";

                   
               }
             
            

           }
           else
           {
               stopKeyAz = true; // сброс ключа стопа
              
               // калибровочное движение, необходимо для выбора направления вращения двигателя
               // работает один раз за время переориентации. ключ сбрасывается вручную
               if (calibrKeyAz == true)
               {


                   calibrKeyAz = false;
                   oldAz = xMagOpt; // запоминаем точку отправки
                   // n2++;
                   serialPort1.WriteLine("1 " + moveDirAz + "255");  // вращаем двигатель на максимальной скорости в предыдущем направлении.
                   dSigmaOldAz = dSigmaAz;
                   calibrOldKeyAz = true;
                   dSigmaOld2Az = dSigmaAz;
                  
               }

               else
               {

                   // двигатель все еще движется в в том же направлении
                   if (Math.Abs(dSigmaAz - dSigmaOld2Az) >= 2 && calibrOldKeyAz == true) // каждые пол градуса посылаем команды управления.
                   {

                       calibrOldKeyAz = false;
                       for (int k1 = 0; k1 <= 300; k1++)
                       {

                           serialPort1.WriteLine("1 0 0"); // тормоз
                       }
                       if (dSigmaAz > dSigmaOldAz) // если ортодрома увеличивается, меняем направление вращения
                       // на противоположное. 
                       {
                           //if (moveDirAz == "0 ") moveDirAz = "1 ";
                           //else moveDirAz = "0 ";

                       }

                   }
                   else
                   {
                       if (Math.Abs(dSigmaAz - dSigmaOld2Az) >= 5) // если ортодрома увеличивается, меняем направление вращения
                       //if (Math.Abs(sigmaSample - xAcelOpt) >= 5)
                       // на противоположное. 
                       {


                           // if (sigmaSample < xAcelOpt)
                           if (dSigmaAz > dSigmaOld2Az)
                           {
                               //n2++;

                               if (moveDirAz == "0 ") moveDirAz = "1 ";
                               else moveDirAz = "0 ";
                           }
                           //sigmaSampleAz = xMagOpt;
                           dSigmaOld2Az = dSigmaAz;
                       }
                       if (Math.Abs(oldAz - xMagOpt) >= 1)
                       // if (Math.Abs(dSigma - dSigmaOld2) >= 1) // каждые 2 градуса посылаем команды управления.
                       {

                           // dsigma - ортодрома, кротчайщее расстояние между двумя точками на сфере.
                           // в нашем случаее - на круге. Преимущество в том, что нет зависимости от
                           // нахождения начальной точки и конечной в разных плоскостях относительно горизонта.
                           // т.е. f1=-30 и f2=30, то dsigma = 60; 



                           // максимальная скорость двигателя, если ортодрома больше 10 градусов
                           if (dSigmaAz >= 10)
                           {


                               serialPort1.WriteLine("1 " + moveDirAz + "255"); // макс скорость

                           }

                           // линейное замедление при ортодроме < 10
                           if (dSigmaAz < 10)
                           {

                               rotSpAz = Convert.ToInt32(135 * (1 - (10 - Math.Abs(az - xMagOpt)) / 10d) + 120); // линейное замедление 
                               if (rotSpAz > 255) rotSpAz = 255;
                               serialPort1.WriteLine("1 " + moveDirAz + Convert.ToString(rotSpAz)); // замедление


                           }

                           oldAz = xMagOpt;
                          

                       }




                   }
               }
           }

           if (driveRotKey == 2 && stopKeyAz == false)
           {
               return true;
           }
           else return false;
       }




       bool ElRot()
       {
           if (dSigmaEl < 1) // если разницамежду заданным и текущим менье 1 градуса, то останавливаемся.
           {

               if (stopKey == true) // отправляем единожды пачку команд на остановку за все время пербывания в этом положении. 
               {
                   stopKey = false;
                   oldEl = xAcelOpt;
                   //n2 = 0;
                   for (int k1 = 0; k1 <= 300; k1++)
                   {

                       serialPort1.WriteLine("0 0 0"); // тормоз
                   }
                   //MessageBox.Show("stop");
                   if (moveDir == "0 ") moveDir = "1 ";
                   else moveDir = "0 ";

                   // n2++;
                   //azOrientComp = false;
                 
               }
             
           }
           else
           {

               stopKey = true; // сброс ключа стопа

               // калибровочное движение, необходимо для выбора направления вращения двигателя
               // работает один раз за время переориентации. ключ сбрасывается вручную
               if (calibrKey == true)
               {


                   calibrKey = false;
                   oldEl = xAcelOpt; // запоминаем точку отправки
                   // n2++;
                   serialPort1.WriteLine("0 " + moveDir + "255");  // вращаем двигатель на максимальной скорости в предыдущем направлении.
                 
                   calibrOldKey = true;
                   dSigmaOld2El = dSigmaEl;
                  
               }

               else
               {

                   // двигатель все еще движется в в том же направлении
                   if (Math.Abs(dSigmaEl - dSigmaOld2El) >= 2 && calibrOldKey == true) // каждые пол градуса посылаем команды управления.
                   {

                       calibrOldKey = false;
                       for (int k1 = 0; k1 <= 300; k1++)
                       {

                           serialPort1.WriteLine("0 0 0"); // тормоз
                       }
                       //if (dSigma > dSigmaOld) // если ортодрома увеличивается, меняем направление вращения
                       //// на противоположное. 
                       //{
                       //    //if (moveDir == "0 ") moveDir = "1 ";
                       //    //else moveDir = "0 ";

                       //}

                   }
                   else
                   {
                       if (Math.Abs(dSigmaEl - dSigmaOld2El) >= 5) // если ортодрома увеличивается, меняем направление вращения
                       //if (Math.Abs(sigmaSample - xAcelOpt) >= 5)
                       // на противоположное. 
                       {


                           // if (sigmaSample < xAcelOpt)
                           if (dSigmaEl > dSigmaOld2El)
                           {
                               // n2++;

                               if (moveDir == "0 ") moveDir = "1 ";
                               else moveDir = "0 ";
                           }
                          // sigmaSample = xAcelOpt;
                           dSigmaOld2El = dSigmaEl;
                       }
                       if (Math.Abs(oldEl - xAcelOpt )>= 1)
                       // if (Math.Abs(dSigma - dSigmaOld2) >= 1) // каждые 2 градуса посылаем команды управления.
                       {

                           // dsigma - ортодрома, кротчайщее расстояние между двумя точками на сфере.
                           // в нашем случаее - на круге. Преимущество в том, что нет зависимости от
                           // нахождения начальной точки и конечной в разных плоскостях относительно горизонта.
                           // т.е. f1=-30 и f2=30, то dsigma = 60; 



                           // максимальная скорость двигателя, если ортодрома больше 10 градусов
                           if (dSigmaEl >= 10)
                           {


                               serialPort1.WriteLine("0 " + moveDir + "255"); // макс скорость

                           }

                           // линейное замедление при ортодроме < 10
                           if (dSigmaEl < 10)
                           {

                               rotSp = Convert.ToInt32(135 * (1 - (10 - Math.Abs(el - xAcelOpt)) / 10d) + 120); // линейное замедление 
                               if (rotSp > 255) rotSp = 255;
                               serialPort1.WriteLine("0 " + moveDir + Convert.ToString(rotSp)); // замедление


                           }

                           oldEl = xAcelOpt;
                           // dSigmaOld2 = dSigma;

                       }




                   }
               }


           }
           if (driveRotKey == 2 && stopKey == false)
           {
               return true;
           }
           else return false;
       }

       void rotatFunc() //valEl - текущее значение, el - заданное значение
       {
            
           //вначале вращаем по азимуту
           // delta = Math.Abs(Math.Abs(el) - Math.Abs(xAcelOpt));
           double f1 = el * Math.PI / 180d;
           double f2 = xAcelOpt * Math.PI / 180d;
           double l1 = az * Math.PI / 180d;
           double l2 = xMagOpt * Math.PI / 180d;
           //dSigmaFull = Math.Acos(Math.Sin(f1) * Math.Sin(f2) + Math.Cos(f1) * Math.Cos(f2) * Math.Cos(l2 - l1)) * 180d / Math.PI;
           dSigmaEl = Math.Acos(Math.Sin(f1) * Math.Sin(f2) + Math.Cos(f1) * Math.Cos(f2)) * 180d / Math.PI;
           dSigmaAz = Math.Acos(Math.Cos(l2-l1)) * 180d / Math.PI;   
           //dSigmaAz = l2 - l1;
          // label13.Invoke(new MethodInvoker(delegate() { label13.Text = moveDir; }));
           //label13.Invoke(new MethodInvoker(delegate() { label13.Text = Convert.ToString(dSigmaAz); }));
         //  label14.Invoke(new MethodInvoker(delegate() { label14.Text = Convert.ToString(dSigmaOld2Az); }));
          // label22.Invoke(new MethodInvoker(delegate() { label22.Text = Convert.ToString(rotSpAz); }));
           label23.Invoke(new MethodInvoker(delegate() { label23.Text = Convert.ToString(n2); }));
           //label24.Invoke(new MethodInvoker(delegate() { label24.Text = mdir; }));

           //ElRot();
          // AzRot();
          // n2++; // счетчик
           //calibrKey = true;
           //calibrKeyAz = true;
           // поворот по азимуту--------------------------
           if (azOrientComp == false)
           {
            
               mdir = "Az";
               if (AzRot() == true)
               {
                   azOrientComp = true;
                   //if (OrientComp == true)
                   //{
                   //    OrientComp = false;
                   //    calibrKey = true; // сбрасываем стоп-ключ элевации
                   //    calibrKeyAz = true; // сбрасываем стоп-ключ азимута
                   //}
               }

           }

           // поворот по элевации-----------------------------------
           if (azOrientComp == true) 
           {
               mdir = "El";
               if (ElRot() == true)
               {
                   azOrientComp = false;
                   //if (OrientComp == false)
                   //{
                   //    OrientComp = true;
                   //    calibrKey = true; // сбрасываем стоп-ключ элевации
                   //    calibrKeyAz = true; // сбрасываем стоп-ключ азимута
                   //}
               }
              
           }
         //  if (ElRot() == true && AzRot() == true) MessageBox.Show("Ready");
        

       }
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            string T;
            string Az;
            string Pitch;
            string Roll,t1,t2 ;
            double AzD = 0, RollD = 0;
           // int AzI = 0;
            
            string bufStr;
            //label5.Invoke(new MethodInvoker(delegate() { label5.Text = Convert.ToString(key); }));
            if (serialPort1.IsOpen)
                Thread.Sleep(40); 
                while (key != true)
                {
                   try
                     { 
                    T = serialPort1.ReadTo("|");
                    Az = serialPort1.ReadTo("|");
                  //  t1 = serialPort1.ReadTo("|");
                    Pitch = serialPort1.ReadTo("|");
                    Roll = serialPort1.ReadTo("|");
                    driveRotKeyNow = Convert.ToInt32(serialPort1.ReadLine());

                    //t2 = serialPort1.ReadLine();
                    //Roll = "76.6";
                    //Az = "346.4";
                    if (Az == "nan") Az = "0.1";
                    bufStr = Az.Replace(".", ","); // заменяем точку на запятую для double
                    
                       AzD = Convert.ToDouble(bufStr);
                       if (AzD < 2 || AzD > 358)
                       {
                           angle = (float)AzD;
                           KalmanFiltrMag(AzD);
                           xMagOpt = AzD;
                       }
                       else
                       {
                           KalmanFiltrMag(AzD);
                           angle = (float)xMagOpt;// значение угла на картинке
                       }
                  

                    bufStr = Roll.Replace(".", ","); // заменяем точку на запятую для double
                    RollD = Convert.ToDouble(bufStr);
                   

                   // xAcelOpt = (float)RollD;
                    KalmanFiltr(RollD);
                    pitch = -(float)xAcelOpt;// значение угла на картинке
                    pictureBox1.Invoke(new MethodInvoker(delegate() { pictureBox1.Invalidate(); }));
                    pictureBox2.Invoke(new MethodInvoker(delegate() { pictureBox2.Invalidate(); }));
                    label10.Invoke(new MethodInvoker(delegate() { label10.Text = T + "°C"; }));
                    label9.Invoke(new MethodInvoker(delegate() { label9.Text = Az; }));
                    label8.Invoke(new MethodInvoker(delegate() { label8.Text = Roll; }));
                    //label7.Invoke(new MethodInvoker(delegate() { label7.Text = Pitch; }));
                    label15.Invoke(new MethodInvoker(delegate() { label15.Text = Convert.ToString(Math.Round(xAcelOpt, 2)) + "°"; }));
                    label18.Invoke(new MethodInvoker(delegate() { label18.Text = Convert.ToString(Math.Round(xMagOpt, 2)) + "°"; }));
                    //label25.Invoke(new MethodInvoker(delegate() { label25.Text = Convert.ToString(driveRotKey); }));
                    //if (orientChan == true)
                    //{
                    //    // calibrKey = true;                        
                    //    rotatFunc(az, el, xMagOpt, xAcelOpt);
                    //}
                       
                   


      }
                    catch (Exception e1)
                    {
                        MessageBox.Show(e1.Message); //если ком порт не открылся, показываем почему.           
                    }

                    //Thread.Sleep(40);
                }
            if (!serialPort1.IsOpen)
           textBox1.Invoke(new MethodInvoker(delegate() { textBox1.Text = "Disconnect"; }));
           // label5.Invoke(new MethodInvoker(delegate() { label5.Text = Convert.ToString(key); }));
           
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            n2+=2;
            if (n2 > 1000) n2 = 0;
            driveRotKey = driveRotKeyNow;
            //pitch  += 0.6f;
            //pictureBox2.Invalidate();
           // double f1 = el * Math.PI / 180d;
           // double f2 = xAcelOpt * Math.PI / 180d;
           // serialPort1.WriteLine("0 1 0"); // тормоз
            //if (calibrKey == false)
            //{

            //    dSigmaOld = Math.Acos(Math.Sin(f1) * Math.Sin(f2) + Math.Cos(f1) * Math.Cos(f2));

            //}
            //dSigmaOld = Math.Acos(Math.Sin(f1) * Math.Sin(f2) + Math.Cos(f1) * Math.Cos(f2)) * 180d / Math.PI;
          


        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label13_Click(object sender, EventArgs e)
        {

        }

        private void button11_Click(object sender, EventArgs e)
        {
            serialPort1.Write("0 0 0");
            orientChan = false;
            //serialPort1.WriteLine("0 0 0");
        }

        private void button8_Click(object sender, EventArgs e)
        {
           
             
            //serialPort1.Write(textBox4.Text + "|" + textBox3.Text + "|");
             az = Convert.ToDouble(textBox4.Text);
             el = Convert.ToDouble(textBox3.Text);
            // delta = Math.Abs(Math.Abs(el) - Math.Abs(xAcelOpt));
             orientChan = true;
             calibrKey = true;
             calibrKeyAz = true;
             azOrientComp = true;
             //calibrOldKey = true;
             //oldEl = xAcelOpt;
             timer1.Stop();
             timer1.Start();
            // MessageBox.Show(Convert.ToString(Math.Sin(1.57)));
        }

        private void button9_Click(object sender, EventArgs e)
        {
            //int i1 = 0;          
            //while (i1 < 10)
            //{
            //    serialPort1.WriteLine("0 0 " + trackBar1.Value);
            //    i1++;
            //}
           
            serialPort1.WriteLine("0 0 " + trackBar1.Value);
            //serialPort1.WriteLine("0 1 " + trackBar1.Value);
            for (int i1 = 0; i1 <= 300; i1++ )
            {
                //    serialPort1.WriteLine("0 1 " + trackBar1.Value);
                serialPort1.WriteLine("0 0 0");
              
            }
           // serialPort1.WriteLine("0 0 0");
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            label21.Text = Convert.ToString(trackBar1.Value);


        }

        private void backgroundWorker2_DoWork(object sender, DoWorkEventArgs e)
        {
           // button9.
            //label18.Invoke(new MethodInvoker(delegate() { label18.Text = Convert.ToString(xMagOpt); }));
            // автоматическая ориентация
            while (true)
           {
            //    if (orientChan == true)
            //    {
            //        // calibrKey = true;                        
            //        rotatFunc(az, el, xMagOpt, xAcelOpt);
            //    }
            //}

            if (orientChan == true)
            {
                // calibrKey = true;                        
                rotatFunc();
               
            }
        }             
        }

        private void button9_MouseDown(object sender, MouseEventArgs e)
        {
            serialPort1.WriteLine("0 0 " + trackBar1.Value);
            //serialPort1.WriteLine("0 0 0");
        }

        private void button9_MouseUp(object sender, MouseEventArgs e)
        {
            int i1 = 0;
            while (i1 < 4)
            {
                //    serialPort1.WriteLine("0 1 " + trackBar1.Value);
                serialPort1.WriteLine("0 0 0");
                i1++;
            }
          //  serialPort1.WriteLine("0 0 0");
        }

        private void button10_Click(object sender, EventArgs e)
        {
           
             serialPort1.WriteLine("0 1 " + trackBar1.Value);
             for (int i1 = 0; i1 <= 300; i1++)
             {
                 //    serialPort1.WriteLine("0 1 " + trackBar1.Value);
                 serialPort1.WriteLine("0 0 0");

             }
            //serialPort1.WriteLine("0 0 0");
        }

        private void button10_MouseDown(object sender, MouseEventArgs e)
        {
            serialPort1.WriteLine("0 1 " + trackBar1.Value);
            //serialPort1.WriteLine("0 0 0");
        }

        private void button10_MouseUp(object sender, MouseEventArgs e)
        {
            int i1 = 0;
            while (i1 < 4)
            {
                //    serialPort1.WriteLine("0 1 " + trackBar1.Value);
                serialPort1.WriteLine("0 0 0");
                i1++;
            }
           // serialPort1.WriteLine("0 0 0");
        }

        private void button5_MouseDown(object sender, MouseEventArgs e)
        {
            serialPort1.WriteLine("1 0 " + trackBar1.Value);
        }

        private void button5_MouseUp(object sender, MouseEventArgs e)
        {
            int i1 = 0;
            while (i1 < 4)
            {
                //    serialPort1.WriteLine("0 1 " + trackBar1.Value);
                serialPort1.WriteLine("1 1 0");
                i1++;
            }
        }

        private void button4_MouseDown(object sender, MouseEventArgs e)
        {
            serialPort1.WriteLine("1 1 " + trackBar1.Value);
        }

        private void button4_MouseUp(object sender, MouseEventArgs e)
        {
            int i1 = 0;
            while (i1 < 4)
            {
                //    serialPort1.WriteLine("0 1 " + trackBar1.Value);
                serialPort1.WriteLine("1 1 0");
                i1++;
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            // получаем список доступных портов
            string[] ports = SerialPort.GetPortNames();
            comboBox1.Items.Clear();
            // выводим список портов
            for (int i = 0; i < ports.Length; i++)
            {
                comboBox1.Items.Add(ports[i].ToString());

            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            orientChan = false;
            button7.Enabled = true;
            timer1.Stop();
            button8.Enabled = false;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            timer1.Interval = 2000;
            button7.Enabled = false;
            //orientChan = true;
            button8.Enabled = true;
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F2 && e.Modifiers == Keys.Alt)
            {
                MessageBox.Show("Hello");
            }
        }

        private void textBox3_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && button8.Enabled == true)
            {
               
                //serialPort1.Write(textBox4.Text + "|" + textBox3.Text + "|");
                az = Convert.ToDouble(textBox4.Text);
                el = Convert.ToDouble(textBox3.Text);
                // delta = Math.Abs(Math.Abs(el) - Math.Abs(xAcelOpt));
                orientChan = true;
                calibrKey = true;
                calibrKeyAz = true;
                azOrientComp = true;
                //calibrOldKey = true;
                //oldEl = xAcelOpt;
                timer1.Stop();
                timer1.Start();
                // MessageBox.Show(Convert.ToString(Math.Sin(1.57)));
            }

        }
        bool keyDownFlag = false;
        private void radioButton1_KeyDown(object sender, KeyEventArgs e)
        {
            
            if (e.KeyCode == Keys.NumPad8 && radioButton1.Checked == true && button9.Enabled == true )
            {
                if (keyDownFlag == false)
                {
                    keyDownFlag = true;
                    serialPort1.WriteLine("0 0 " + trackBar1.Value);
                }
                         
            }
            if (e.KeyCode == Keys.NumPad2 && radioButton1.Checked == true && button9.Enabled == true)
            {
                if (keyDownFlag == false)
                {
                    keyDownFlag = true;
                    serialPort1.WriteLine("0 1 " + trackBar1.Value);
                }
            }
            if (e.KeyCode == Keys.NumPad4 && radioButton1.Checked == true && button9.Enabled == true)
            {
                if (keyDownFlag == false)
                {
                    keyDownFlag = true;
                    serialPort1.WriteLine("1 1 " + trackBar1.Value);
                }
               
            }
            if (e.KeyCode == Keys.NumPad6 && radioButton1.Checked == true && button9.Enabled == true)
            {
                if (keyDownFlag == false)
                {
                    keyDownFlag = true;
                    serialPort1.WriteLine("1 0 " + trackBar1.Value);
                }
                //for (int i1 = 0; i1 <= 300; i1++)
                //{
                //    //    serialPort1.WriteLine("0 1 " + trackBar1.Value);
                //    serialPort1.WriteLine("1 0 0");

                //}
            }

          
            if (e.KeyCode == Keys.NumPad5 && radioButton1.Checked == true && button9.Enabled == true)
            {

                for (int i1 = 0; i1 <= 300; i1++)
                {
                    //    serialPort1.WriteLine("0 1 " + trackBar1.Value);
                    serialPort1.WriteLine("0 0 0");

                }
                orientChan = false;
            }
        }

        private void radioButton1_KeyUp(object sender, KeyEventArgs e)
        {
            keyDownFlag = false;
            for (int i1 = 0; i1 <= 300; i1++)
            {
                //    serialPort1.WriteLine("0 1 " + trackBar1.Value);
                serialPort1.WriteLine("1 0 0");

            }
        }

        
        private void trackBar1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.NumPad8 && radioButton1.Checked == true && button9.Enabled == true)
            {
                if (keyDownFlag == false)
                {
                    keyDownFlag = true;
                    serialPort1.WriteLine("0 0 " + trackBar1.Value);
                }

            }
            if (e.KeyCode == Keys.NumPad2 && radioButton1.Checked == true && button9.Enabled == true)
            {
                if (keyDownFlag == false)
                {
                    keyDownFlag = true;
                    serialPort1.WriteLine("0 1 " + trackBar1.Value);
                }
            }
            if (e.KeyCode == Keys.NumPad4 && radioButton1.Checked == true && button9.Enabled == true)
            {
                if (keyDownFlag == false)
                {
                    keyDownFlag = true;
                    serialPort1.WriteLine("1 1 " + trackBar1.Value);
                }

            }
            if (e.KeyCode == Keys.NumPad6 && radioButton1.Checked == true && button9.Enabled == true)
            {

                if (keyDownFlag == false)
                {
                    keyDownFlag = true;
                    serialPort1.WriteLine("1 0 " + trackBar1.Value);
                }
                //for (int i1 = 0; i1 <= 300; i1++)
                //{
                //    //    serialPort1.WriteLine("0 1 " + trackBar1.Value);
                //    serialPort1.WriteLine("1 0 0");

                //}
            }


            if (e.KeyCode == Keys.NumPad5 && radioButton1.Checked == true && button9.Enabled == true)
            {

                for (int i1 = 0; i1 <= 300; i1++)
                {
                    //    serialPort1.WriteLine("0 1 " + trackBar1.Value);
                    serialPort1.WriteLine("0 0 0");

                }
                orientChan = false;
            }
        }

        private void trackBar1_KeyUp(object sender, KeyEventArgs e)
        {
            keyDownFlag = false;
            for (int i1 = 0; i1 <= 300; i1++)
            {
                ;
                //    serialPort1.WriteLine("0 1 " + trackBar1.Value);
                serialPort1.WriteLine("0 0 0");

            }
        }
    }
}
