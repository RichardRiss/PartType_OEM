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

namespace PartType_OEM
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
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

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

    }
}
