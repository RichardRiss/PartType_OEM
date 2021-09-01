using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Threading;
using Sharp7;



namespace PartType_OEM
{

    public partial class Form1 : Form
    {
        //Global Definition
        private bool connected;
        S7Client client = new S7Client();
        S7Client.S7CpInfo info;

        public Form1()
        {
            InitializeComponent();
            this.FormClosing += UserClosingEvent;
        }


        //Reverse Byte Order 16-Bit 
        public static UInt16 ReverseBytes(UInt16 value)
        {
            return (UInt16)((value & 0xFFU) << 8 | (value & 0xFF00U) >> 8);
        }

        //Reverse Byte Order 32-Bit 
        public static UInt32 ReverseBytes(UInt32 value)
        {
            return (value & 0x000000FFU) << 24 | (value & 0x0000FF00U) << 8 |
                   (value & 0x00FF0000U) >> 8 | (value & 0xFF000000U) >> 24;
        }

        //Reverse Byte Order 64-Bit 
        public static UInt64 ReverseBytes(UInt64 value)
        {
            return (value & 0x00000000000000FFUL) << 56 | (value & 0x000000000000FF00UL) << 40 |
                   (value & 0x0000000000FF0000UL) << 24 | (value & 0x00000000FF000000UL) << 8 |
                   (value & 0x000000FF00000000UL) >> 8 | (value & 0x0000FF0000000000UL) >> 24 |
                   (value & 0x00FF000000000000UL) >> 40 | (value & 0xFF00000000000000UL) >> 56;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //Launch second Thread
            Thread plcconnection = new Thread(new ThreadStart(PlcConnector));
            plcconnection.Start();

            // TODO: Diese Codezeile lädt Daten in die Tabelle "partdataDataSet.parts". Sie können sie bei Bedarf verschieben oder entfernen.
            this.partsTableAdapter.Fill(this.partdataDataSet.parts);
            this.gui_parts_table.RowHeadersVisible = false;
            this.gui_parts_table.AllowUserToResizeRows = false;
            this.gui_parts_table.AllowUserToResizeColumns = false;
            this.gui_parts_table.MultiSelect = false;
            this.gui_parts_table.BackgroundColor = Color.White;
            var con = this.partsTableAdapter.Connection;
            if (con != null && con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            var active_type = "SELECT partnumber from parts WHERE activated = 1;";
            var cmd_act = new MySqlCommand(active_type, con);
            var num = cmd_act.ExecuteScalar();
            foreach (DataGridViewRow row in this.gui_parts_table.Rows)
            {
                if (Convert.ToInt32(row.Cells[0].Value.ToString()) == Convert.ToInt32(num))
                {
                    row.DefaultCellStyle.BackColor = Color.Yellow;
                }
            }
        }


        public void onTimedEvent(object sender, EventArgs e) 
        {
            int Rack = 0;
            int Slot = 2;
            string Address = "192.168.214.1";


            if (connected)
            {

                int result = client.Connect();
                if (result != 0)
                {
                    connected = false;
                }
            }
            else 
            {
                int result = client.ConnectTo(Address, Rack, Slot);
                if (result == 0)
                {
                    connected = true;
                    Console.WriteLine("Connected Successfully.");
                }
                else
                {
                    connected = false;
                    Console.WriteLine("Failed on Connection Establishment");
                }
            }


        }

        private static void readNames(S7Client client, ref List<string> names, ref bool error)
        {
            //Create Config Read
            int area = (int) S7Area.DB;
            int bytelen = (int)S7WordLength.Byte;

            //16 chars x 1000 types
            byte[] type_1_1000 = new byte[16000];
            int bytes_read = new int();

            //Perform Read
            var watch = System.Diagnostics.Stopwatch.StartNew();
            int result = client.ReadArea(area, 260, 15612, 16000, bytelen, type_1_1000, ref bytes_read);
            watch.Stop();
            Console.WriteLine($"Read of Names took {watch.ElapsedMilliseconds} ms");
            error = result != 0 | bytes_read != type_1_1000.Length;

            for(int i = 0; i < type_1_1000.Length; i += 16)
            {
                int rangeend = 16;
                string utf8string = Encoding.UTF8.GetString(type_1_1000, i, rangeend);
                utf8string = utf8string.Trim('\0');
                utf8string = utf8string.Trim();
                names.Add(utf8string);

            }




        }


        private static void  readTypes(S7Client client, ref List<int> types, ref bool error)
        {
            //Create Read Config
            int wordlen = (int)S7WordLength.Word;
            int area = (int)S7Area.DB;

            //Type 1-32 DB259.DBW2922: 32 Word
            byte[] type_1_32 = new byte[64];
            int bytes_read_1 = new int();


            //Type 33-1000 DB260.DBW13522: 968 Word
            byte[] type_33_1000 = new byte[1936];
            int bytes_read_2 = new int();

            //Perform Read
            var watch = System.Diagnostics.Stopwatch.StartNew();
            int result1 = client.ReadArea(area, 259, 2922, 32, wordlen, type_1_32, ref bytes_read_1);
            int result2 = client.ReadArea(area, 260, 13552, 968, wordlen, type_33_1000, ref bytes_read_2);
            watch.Stop();
            Console.WriteLine($"Read of Types took {watch.ElapsedMilliseconds} ms");
            error = result1 != 0 | result2 != 0 | bytes_read_1 != type_1_32.Length | bytes_read_2 != type_33_1000.Length;

            var arr_conc = type_1_32.Concat(type_33_1000).ToArray();

            for(int i = 0; i < arr_conc.Length; i+=(wordlen/2))
            {
                types.Add(ReverseBytes(BitConverter.ToUInt16(arr_conc, i)));
            }
 
        }


        public void PlcConnector()
        {
            System.Timers.Timer aTimer = new System.Timers.Timer();
            aTimer.Elapsed += new System.Timers.ElapsedEventHandler(onTimedEvent);
            aTimer.Interval = 5000;
            aTimer.Enabled = true;




            while (true)
            {
                if (connected)
                {
                    //Read PDU Size (240-960Byte)
                    client.GetCpInfo(ref info);
                    Console.WriteLine($"Max PDU Length: {info.MaxPduLength}");

                    List<int> types = new List<int>();
                    List<string> names = new List<string>();
                    bool error = false;
                    readTypes(client, ref types, ref error);
                    readNames(client, ref names, ref error);
                    Console.WriteLine(error);
                    
                    break;

                }
                else
                {
                    Console.WriteLine("Waiting for Connection Establishment");
                }

                Thread.Sleep(1000);
            }


        }


        private void button1_Click(object sender, EventArgs e)
        {
            var con = this.partsTableAdapter.Connection;
            if (con != null && con.State == ConnectionState.Closed)
            {
                con.Open();
            }

            var selection_val = gui_parts_table.Rows[gui_parts_table.CurrentRow.Index].Cells[0].Value;
            var selected_num = Convert.ToInt32(selection_val);
            
            
            //Reset Value in DB
            var stm = "UPDATE parts SET activated = 0 WHERE activated = 1;";
            var cmd = new MySqlCommand(stm, con);
            cmd.ExecuteScalar();

            //Reset Color in DataGridView
            foreach (DataGridViewRow row in this.gui_parts_table.Rows)
            {
                row.DefaultCellStyle.BackColor = Color.White;
            }


            var query_num = $"UPDATE parts SET activated = 1 WHERE partnumber = {selected_num};";
            var command_num = new MySqlCommand(query_num, con);
            var retval = command_num.ExecuteNonQuery();
            this.gui_parts_table.CurrentRow.DefaultCellStyle.BackColor = Color.Yellow;
            if (retval > 1)
            {
                Console.WriteLine("Error multiple Parts selected");
                cmd.ExecuteNonQuery();
                //Reset Color in DataGridView
                foreach (DataGridViewRow row in this.gui_parts_table.Rows)
                {
                    row.DefaultCellStyle.BackColor = Color.White;
                }
            }

        }


        protected void UserClosingEvent(object sender,EventArgs e)
        {
            client.Disconnect();
            Console.WriteLine("Connection Closed");
            Environment.Exit(Environment.ExitCode);
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

    }
}
