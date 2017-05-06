using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Subneteo
{
    public partial class Form1 : Form
    {
        String tipoMAC;
        string ip,mascara;
        double verificador = 0;
        string cat1masc, cat2masc, cat3masc, cat4masc;
        //4294967296
        public Form1()
        {
            InitializeComponent();
        }
        public void Msgbox(String cadena)
        {
            MessageBox.Show(cadena);
        }
        private void button1_Click(object sender, EventArgs e)
        {   //CO
            if (System.Convert.ToInt16(TXT_OCT1IP.Text) > 255)
            {
                Msgbox("Error");
                TXT_OCT1IP.Focus();
            }
            else if (System.Convert.ToInt16(TXT_OCT2IP.Text) > 255)
            {
                Msgbox("Error");
                TXT_OCT2IP.Focus();
            }
            else if (System.Convert.ToInt16(TXT_OCT3IP.Text) > 255)
            {
                Msgbox("Error");
                TXT_OCT3IP.Focus();
            }
            else if (System.Convert.ToInt16(TXT_OCT4IP.Text) > 255)
            {
                Msgbox("Error");
                TXT_OCT4IP.Focus();
            }
            else
            {
                ip = convertirbinario(TXT_OCT1IP.Text) + "." + convertirbinario(TXT_OCT2IP.Text) + "." + convertirbinario(TXT_OCT3IP.Text) + "." + convertirbinario(TXT_OCT4IP.Text);
                crear_mascara();
                GenerarTipoMAC();
                TXT_MASCARA.Text = STRbintoint(mascara.Substring(0, 8)).ToString() + "." + STRbintoint(mascara.Substring(9, 8)).ToString() + "." + STRbintoint(mascara.Substring(18, 8)).ToString() + "." + STRbintoint(mascara.Substring(27, 8)).ToString();
                cat1masc = STRbintoint(mascara.Substring(0, 8)).ToString();
                cat2masc = STRbintoint(mascara.Substring(9, 8)).ToString();
                cat3masc = STRbintoint(mascara.Substring(18, 8)).ToString();
                cat4masc = STRbintoint(mascara.Substring(27, 8)).ToString();
                GenerarIPS();
            }


        }
        public void GenerarIPS()
        {
            int subs = 2;
            int hosts = 2;
            
            



            int contadorini;
            string ipini = TXT_OCT1IP.Text;
            if (!(cat2masc == "255" || cat2masc == "0"))
            {
                subs = Convert.ToInt32(Math.Pow(2, 24 - verificador));
                contadorini = 256 / Convert.ToInt32(subs);
            }
            else if (!(cat3masc == "255" || cat3masc == "0"))
            {//IPS DE CLASE B
                hosts = 2;//CAMBIAR ESTA MAL
                ipini = ipini + "." + TXT_OCT2IP.Text;
                contadorini = 256 / Convert.ToInt32(hosts);
                int broadcast = 255;
                int inicial = 0;
                int medio1 = 0, medio2 = 0;
                string IPInsert1 = ipini;
                string IPInsert2 = ipini;
                String IPInsert3 = ipini;
                int i = 0;
                while (broadcast <= 255)
                {
                    medio2 = Convert.ToInt32(medio2) + hosts;
                    IPInsert1 = IPInsert1 + "." + medio1.ToString() + "." + (inicial).ToString();
                    IPInsert2 = IPInsert2 + "." + medio2.ToString() + "." + (broadcast).ToString();
                    IPInsert3 = IPInsert3 + "." + medio1.ToString() + "." + (inicial + 1).ToString() + " - " + IPInsert3 + "." + medio2.ToString() + "." + (broadcast - 1).ToString();
                    dataGridView1.Rows.Add(i + 1, IPInsert1, IPInsert2, IPInsert3);
                    medio1 = medio2;
                }
            }
            else //IPS DE CLASE C
            {
                if (tipoMAC == "C") //Comprobamos que la IP sea tipo C
                {
                    subs = Convert.ToInt32(Math.Pow(2, verificador));
                    hosts = Convert.ToInt32(Math.Pow(2, 8 - verificador));
                }
                if (tipoMAC == "B")
                {
                    subs = Convert.ToInt32(Math.Pow(2, verificador));
                    hosts = Convert.ToInt32(Math.Pow(2, 16 - verificador));
                }
                ipini = ipini + "." + TXT_OCT2IP.Text + "." + TXT_OCT3IP.Text;
                contadorini = 256 / Convert.ToInt32(hosts);

                for (int i = 0; i < hosts; i++)
                {
                    int broadcast;
                    int inicial;
                    string IPInsert1 = ipini;
                    string IPInsert2 = ipini;
                    String IPInsert3 = ipini;
                    broadcast = (i + 1) * contadorini - 1;
                    inicial = i * contadorini;
                    IPInsert1 = IPInsert1 + "." + (inicial).ToString();
                    IPInsert2 = IPInsert2 + "." + (broadcast).ToString();
                    IPInsert3 = IPInsert3 + "." + (inicial + 1).ToString() + " - " + IPInsert3 + "." + (broadcast - 1).ToString();
                    dataGridView1.Rows.Add(i + 1, IPInsert1, IPInsert2, IPInsert3);
                }
            }
        }
        private String Reemplazo(int posicion, String original, String reemplazo)
        {//Reemplazo(6,"Archivo","b"); == Archibo
            String primeracad = "";
            String segundacad = "";
            String nuevacad = "";
            primeracad = original.Substring(0, posicion - 1);
            segundacad = original.Substring(posicion, original.Length - posicion);
            nuevacad = primeracad + reemplazo + segundacad;
            return nuevacad;
        }
        private int generarN(int numero)
        {
            int i;
            for (i = 0; i < 50; i++)
            {
                double pot = Math.Pow(2,Convert.ToDouble(i));
                if (numero <= Convert.ToInt32(pot))
                {
                    return i;
                }
            }
            return 0;
        }
        
        private void GenerarTipoMAC()
        {
            int primeroct = Convert.ToInt32(TXT_OCT1IP.Text);
            if (primeroct < 128)
                tipoMAC = "A";
            else if (primeroct < 192)
                tipoMAC = "B";
            else if (primeroct < 224)
                tipoMAC = "C";
            else if (primeroct < 240)
                tipoMAC = "D";
            else
                tipoMAC = "E";
        }
        private void TXT_OCT1IP_Leave()
        {

        }
        private int completarN(int N)
        {
            if (N < 8)
            {
                return 8;
            }
            else if (N<16)
            {
                return 16;
            }
            else if (N < 24)
            {
                return 24;
            }
            return 0;
        }
        private int STRbintoint(string cadena)
        {
            int numero = 0;
            for (double i = 0; i < 8; i++)
            {
                if (cadena.Substring(Convert.ToInt32(i), 1) == "1")
                {
                    numero = numero + Convert.ToInt32(Math.Pow(2, 7 - i));
                }
            }
            return numero;
        }
        public void crear_mascara()
        {
            mascara = ip;
            int contador = 0;
            if (rb_hosts.Checked == true)
            {
                int N = generarN(Convert.ToInt32(txt_hosts_subredes.Text) + 2);
                verificador = N;
                for (int x = 0; x < completarN(N); x++)
                {
                    if (mascara.Substring(34 - contador, 1) != ".")
                    {
                        if (contador >= N)
                        {
                            mascara = Reemplazo(35 - contador, mascara, "1");
                        }
                        else
                        {
                            mascara = Reemplazo(35 - contador, mascara, "0");
                        }
                    }
                    contador++;
                }
                for (int i = contador; i < 35; i++)
                {
                    if (mascara.Substring(34 - contador, 1) != ".")
                    {
                        mascara = Reemplazo(35 - contador, mascara, "1");
                    }
                    contador++;
                }
            }
            else
            {
                int N = generarN(Convert.ToInt32(txt_hosts_subredes.Text));
                int iteraciones = completarN(N);
                verificador = N;
                N = completarN(N) - N;
                for (int x = 0; x < iteraciones; x++)
                {

                    if (mascara.Substring(34 - contador, 1) != ".")
                    {
                        if (contador >= N)
                        {
                            mascara = Reemplazo(35 - contador, mascara, "1");
                        }
                        else
                        {
                            mascara = Reemplazo(35 - contador, mascara, "0");
                        }
                        contador++;
                    }

                }
                for (int i = contador; i < 35; i++)
                {
                    if (mascara.Substring(34 - contador, 1) != ".")
                    {
                        mascara = Reemplazo(35 - contador, mascara, "1");
                    }
                    contador++;
                }
                
            }
        }
      
        private string convertirbinario(string octeto)
        {
            double valor = Convert.ToDouble(octeto);
            string ip_temporal = "";
            while ((Convert.ToInt32(valor) > 0) || (ip_temporal.Length < 8))
            {
                int residuo = Convert.ToInt32(valor % 2);
                ip_temporal = residuo + ip_temporal;
                valor = Math.Truncate(valor / 2);
            }
            return ip_temporal;
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        
        }
        private int convertir_decimal()
        {
           


            return 0;
        }
    }
}
