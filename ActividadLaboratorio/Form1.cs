using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace ActividadLaboratorio
{
    public partial class Form1 : Form
    {
        public List<(string nombre, int x, int y)> listaPuntos = new List<(string nombre, int x, int y)>();
        public List<(string nombre, string puntoinicio, string puntofinal)> listaLineas = new List<(string nombre, string puntoinicio, string puntofinal)>();

        public Form1()
        {
            InitializeComponent();
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Graphics grafica = this.pictureBox1.CreateGraphics();
            SolidBrush Brush = new SolidBrush(Color.Black);

            // Crea la posición y tamaño del círculo
            int Enx = (int)numericUpDown1.Value;
            int Eny = (int)numericUpDown2.Value;
            int ancho = 10;
            int alto = 10;

            // revisa que no exista el punto
            if (listaPuntos.Any(z => (z.nombre == textBox1.Text)))
            {
                MessageBox.Show("El punto " + textBox1.Text + " ya existe");
                textBox1.Text = string.Empty;
                numericUpDown1.Value = 0;
                numericUpDown2.Value = 0;
                return;
            }

            // revisa que no existan las coordenadas 
            if (listaPuntos.Any(z => (z.x == Enx && z.y == Eny)))
            {
                MessageBox.Show("Las coordenadas proporcionadas ya existen");
                textBox1.Text = string.Empty;
                numericUpDown1.Value = 0;
                numericUpDown2.Value = 0;
                return;
            }


            // Agrega en la lista de puntos
            (string nombre, int x, int y) puntos = (textBox1.Text, Enx, Eny); // crea una tupla
            listaPuntos.Add(puntos);

            comboBox1.Items.Add(textBox1.Text); // agrega en ambos 
            comboBox2.Items.Add(textBox1.Text); // agrega en ambos 


            // Dibuja un círculo
            grafica.FillEllipse(Brush, Enx, Eny, ancho, alto);
            // Coloca el nombre del Punto
            grafica.DrawString(textBox1.Text,
                               new Font("Arial", 10),
                               System.Drawing.Brushes.Gray,
                               new Point(Enx + 10, Eny + 10));

        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {

        }

        private void numericUpDown4_ValueChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            string punto1, punto2;
            int Indicep1, Indicep2;

            if (comboBox1.Text == "" || comboBox2.Text == "")
            {
                MessageBox.Show("El punto inicial y final deben definirse");
                return;
            }
            if (comboBox1.Text == comboBox2.Text)
            {
                MessageBox.Show("El punto inicial y final deben de ser distintos");
                return;
            }

            punto1 = comboBox1.Text;
            punto2 = comboBox2.Text;


            Indicep1 = listaPuntos.FindIndex(x => x.nombre == punto1);
            Indicep2 = listaPuntos.FindIndex(x => x.nombre == punto2);


            // revisa que no exista esa línea
            if (listaLineas.Any(x => (x.puntoinicio == punto1 && x.puntofinal == punto2)) ||
                listaLineas.Any(x => (x.puntoinicio == punto2 && x.puntofinal == punto1)))
            {
                MessageBox.Show("La línea " + textBox2.Text + " ya existe");
                textBox2.Text = String.Empty;
                comboBox1.Text = String.Empty;
                comboBox2.Text = String.Empty;
                return;
            }

            // Agrega a la lista de líneas
            (string nombre, string puntoinicio, string puntofinal) linea = (textBox2.Text, punto1, punto2); // crea una tupla
            listaLineas.Add(linea);



            Font font = new Font("new roman", 8);
            Graphics grafica = this.pictureBox1.CreateGraphics();
            SolidBrush Brush = new SolidBrush(Color.Black);
            Pen pen = new Pen(Color.Black, 4);
            SolidBrush brush = new SolidBrush(Color.Blue);

            Point p1 = new Point();
            Point p2 = new Point();
            p1.X = listaPuntos[Indicep1].x + 5;
            p1.Y = listaPuntos[Indicep1].y + 5;
            p2.X = listaPuntos[Indicep2].x + 5;
            p2.Y = listaPuntos[Indicep2].y + 5;

            // calcula donde colocar el nombre de la línea
            Point K1 = new Point();
            Point K2 = new Point();
            K1.X = (p1.X + p2.X + 10) / 2;
            K2.Y = (p1.Y + p2.Y + 10) / 2;

            grafica.DrawLine(pen, p1, p2);
            grafica.DrawString(textBox2.Text, font, brush, K1.X, K2.Y);
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            string smatriz = "";
            int indicep1, indicep2, peso;
            int totaldevertices = 0;
            textBox3.Text = "";

            totaldevertices = listaPuntos.Count;
            if (totaldevertices == 0)
            {
                MessageBox.Show("No hay información para crear un grafo");
                return;
            }

            // Crea la matriz y la inicializa con 0 porque es un grafo no dirigido
            int[,] matrizAdjacencia = new int[totaldevertices, totaldevertices];
            for (int x = 0; x < totaldevertices; x++)
                for (int y = 0; y < totaldevertices; y++)
                    matrizAdjacencia[x, y] = 0;

            // Llena a partir de la lista de líneas
            foreach ((string n, string Pinicio, string Puntofinal) in listaLineas)
            {
                indicep1 = listaPuntos.FindIndex(z => z.nombre == Pinicio);
                indicep2 = listaPuntos.FindIndex(z => z.nombre == Puntofinal);
                matrizAdjacencia[indicep1, indicep2] = 1;
                matrizAdjacencia[indicep2, indicep1] = 1;
            }


            // Puede copiarse uno a uno los datos, o indicarle de donde tomar la fuente de datos
            listBox1.DataSource = listaPuntos.Select(t => t.nombre).ToList();
            listBox2.DataSource = listaLineas.Select(t => t.nombre).ToList();

            smatriz = " ";
            for (int x = 0; x < totaldevertices; x++)
                smatriz = smatriz + "\t" + listBox1.Items[x].ToString();

            smatriz = smatriz + "\r\n";
            for (int x = 0; x < totaldevertices; x++)
            {
                smatriz = smatriz + listBox1.Items[x].ToString() + "| [ ";
                for (int y = 0; y < totaldevertices; y++)
                {
                    peso = matrizAdjacencia[x, y];
                    smatriz = smatriz + "\t" + matrizAdjacencia[x, y].ToString();
                }
                smatriz = smatriz + " ]\r\n";
            }

            textBox3.Text = smatriz;

        }

        private void button4_Click(object sender, EventArgs e)
        {
            void Amplitud(long[,] MatrizdeAdyancencia, int dimensionmatriz, int nodoinicial)
            {
                int[] padre = new int[dimensionmatriz];
                int[] color = new int[dimensionmatriz]; // 0 = blanco, 1 = gris, 2 = negro
                int v;
                string recorrido = "";

                Queue pendiente = new Queue(dimensionmatriz);
                for (int x = 0; x < dimensionmatriz; x++)
                    color[x] = 0;

                padre[nodoinicial] = -1;               // no tiene
                pendiente.Enqueue(nodoinicial);        // lo agrega la la cola

                while (pendiente.Count > 0)             // Mientras que la cola no esté vacía
                {
                    v = Convert.ToInt32(pendiente.Dequeue());   // elimina un elemento de la cola

                    recorrido = recorrido + listaPuntos.Select(t => t.nombre).ToList() + "\n";  // visita Robin: (puede que este mal por el nombre del nodo que visita, revisar)

                    for (int x = 0; x < dimensionmatriz; x++)
                        if (MatrizdeAdyancencia[v, x] != 0)
                        {
                            if (color[x] == 0)            // Busca nodos blancos
                            {
                                color[x] = 1;             // cambiar a color gris
                                pendiente.Enqueue(x);     // Agrega a la cola
                                padre[x] = v;
                            }
                        }
                    color[v] = 2; // cambia a negro
                }
                MessageBox.Show("Recorrido en Amplitud:\n" + recorrido);
            }

        }

        private void button5_Click(object sender, EventArgs e)
        {
            void Profundidad(long[,] MatrizdeAdyancencia, int dimensionmatriz, int nodoinicial)
            {
                int[] padre = new int[dimensionmatriz];
                int[] color = new int[dimensionmatriz]; // 0 = blanco, 1 = gris, 2 = negro
                int v;
                string recorrido = "";

                Stack pendiente = new Stack(dimensionmatriz);
                for (int x = 0; x < dimensionmatriz; x++)
                    color[x] = 0;

                padre[nodoinicial] = -1;      // no tiene
                pendiente.Push(nodoinicial);  // coloca el punto inicial en la pila

                while (pendiente.Count > 0)      // Mientras que la pila no esté vacía
                {
                    v = Convert.ToInt32(pendiente.Pop());     // saca un elemento de la pila

                    recorrido = recorrido + listaPuntos.Select(t => t.nombre).ToList() + "\n";  // lo visita Robin: (puede que este mal por el nombre del nodo que visita, revisar)

                    for (int x = 0; x < dimensionmatriz; x++)
                        if (MatrizdeAdyancencia[v, x] != 0)
                        {
                            if (color[x] == 0)          // busca los nodos blancos
                            {
                                color[x] = 1;           // cambiar a color gris
                                pendiente.Push(x);      // agregar a la pila
                                padre[x] = v;
                            }
                        }
                    color[v] = 2; // cambia a negro
                }
                MessageBox.Show("Recorrido en Profundidad:\n" + recorrido);
            }

        }

        private void button6_Click(object sender, EventArgs e)
        {

        }
    }
}
