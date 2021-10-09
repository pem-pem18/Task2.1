using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

namespace Task2._1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        
        // клик по кнопке <добавить значения боксов в соответствующие столбцы datagridview>
        private void AddButton_Click(object sender, EventArgs e)
        {
            if (IDBox.Text == "" || ClientBox.Text == "" || PhoneNumberBox.Text == "" || EmailBox.Text == "")
            {
                MessageBox.Show("Недопустимое значение поля.", "Ошибка"); // проверка на пустоту боксов
            }
            else
            {
                int n = 0;

                //if (dataGridView.Rows.Count == 0)
                //    n = dataGridView.Rows.Add(); // взятие индекса каждой новой строки
                //else
                //{
                //    n = dataGridView.Rows.Count;

                //    //// загрузка файла в объект
                //    //XDocument xDoc = XDocument.Load("Test.xml");
                //    //foreach (XElement root in xDoc.Descendants("NewDataSet"))
                //    //    n = root.Elements("Contract").Count();
                //}

                n = dataGridView.Rows.Add(); // взятие индекса каждой новой строки

                dataGridView.Rows[n].Cells[0].Value = IDBox.Text;
                dataGridView.Rows[n].Cells[1].Value = ClientBox.Text;
                dataGridView.Rows[n].Cells[2].Value = PhoneNumberBox.Text;
                dataGridView.Rows[n].Cells[3].Value = EmailBox.Text;

                // dataGridView.Rows.Add(IDBox.Text, ClientBox.Text, PhoneNumberBox.Text, EmailBox.Text);

                // очистка боксов для последующего ввода
                IDBox.Clear();
                ClientBox.Clear();
                PhoneNumberBox.Clear();
                EmailBox.Clear();
            }
        }

        // клик по кнопке <очиститть строки datagridview>
        private void ClearButton_Click(object sender, EventArgs e)
        {
            dataGridView.Rows.Clear();
        }

        // клик по кнопке <импорт из файла в datagridview>
        private void ImportButton_Click(object sender, EventArgs e)
        {
            try
            {
                // загрузка файла в объект
                XDocument xDoc = XDocument.Load("Test.xml");

                // обход всех узлов в корневом элементе
                foreach (XElement root in xDoc.Descendants("NewDataSet"))
                {
                    if (root.HasElements)
                    {
                        dataGridView.Rows.Clear();

                        // обходим все дочерние узлы элемента Contract
                        foreach (XElement elm in xDoc.Descendants("Contract"))
                        {
                            // чтение элементов
                            if (elm.Element("ID") != null && elm.Element("Client") != null && elm.Element("PhoneNumber") != null && elm.Element("Email") != null)
                                dataGridView.Rows.Add(elm.Element("ID").Value, elm.Element("Client").Value, elm.Element("PhoneNumber").Value, elm.Element("Email").Value);
                        }
                    }
                }

                ////создаём таблицу
                //DataTable dt = new DataTable("NewDataSet");

                ////создаём 4 колонки
                //DataColumn ID = new DataColumn("ID", typeof(String));
                //DataColumn Client = new DataColumn("Client", typeof(String));
                //DataColumn PhoneNumber = new DataColumn("PhoneNumber", typeof(String));
                //DataColumn Email = new DataColumn("Email", typeof(String));

                ////добавляем колонки в таблицу
                //dt.Columns.Add(ID);
                //dt.Columns.Add(Client);
                //dt.Columns.Add(PhoneNumber);
                //dt.Columns.Add(Email);

                //DataRow newRow = null;

                //// загрузка файла в объект
                //XDocument xDoc = XDocument.Load("Test.xml");

                //// обход всех узлов в корневом элементе
                //foreach (XElement root in xDoc.Descendants("NewDataSet"))
                //{
                //    if (root.HasElements)
                //    {
                //        // обходим все дочерние узлы элемента Contract
                //        foreach (XElement elm in xDoc.Descendants("Contract"))
                //        {
                //            newRow = dt.NewRow();

                //            // чтение каждого элемента
                //            if (elm.Element("ID") != null)
                //                newRow["ID"] = int.Parse(elm.Element("ID").Value);
                //            if (elm.Element("Client") != null)
                //                newRow["Client"] = int.Parse(elm.Element("Client").Value);
                //            if (elm.Element("PhoneNumber") != null)
                //                newRow["PhoneNumber"] = int.Parse(elm.Element("PhoneNumber").Value);
                //            if (elm.Element("Email") != null)
                //                newRow["Email"] = int.Parse(elm.Element("Email").Value);

                //            dataGridView.Columns.Clear(); // очистка столбцов datagridview

                //            dt.Rows.Add(newRow); //добавляем новую запись в таблицу
                //        }
                //    }
                //}
                //dataGridView.DataSource = dt; // добавляем в datagrid

                // // отвязываем данные от dgv
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка"); // вывод ошибки
            }
        }

        // клик по кнопке <сохранить данные в XML>
        private void SaveButton_Click(object sender, EventArgs e)
        {
            try
            {
                DataSet ds = new DataSet(); // создаем пустой кэш данных
                DataTable dt = new DataTable(); // создаем пустую таблицу данных

                dt.TableName = "Contract";
                dt.Columns.Add("ID");
                dt.Columns.Add("Client");
                dt.Columns.Add("PhoneNumber");
                dt.Columns.Add("Email");

                ds.Tables.Add(dt);
                
                foreach (DataGridViewRow r in dataGridView.Rows) // пока в datagridview есть строки
                {
                    DataRow row = ds.Tables["Contract"].NewRow(); // создаем новую строку в таблице, занесенной в ds

                    // в столбец каждой из строк заносим значение соответствующего стоблца datagridview
                    row["ID"] = r.Cells[0].Value;
                    row["Client"] = r.Cells[1].Value;
                    row["PhoneNumber"] = r.Cells[2].Value;
                    row["Email"] = r.Cells[3].Value;

                    ds.Tables["Contract"].Rows.Add(row);
                }
                ds.WriteXml("Test.xml");
                MessageBox.Show("XML файл успешно сохранен.", "Выполнено");
            }
            catch
            {
                MessageBox.Show("Невозможно сохранить XML файл.", "Ошибка");
            }
        }
    }
}
