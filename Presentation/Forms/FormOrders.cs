using Domain.Models;
using Domain.States;
using Presentation.Helps;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Presentation.Forms
{
    public partial class FormOrders : Form
    {
        private OrderModel order = new OrderModel();

        public FormOrders()
        {
            InitializeComponent();
            panel1.Enabled = false;
        }

        private void ListOrders()
        {
            try
            {
                dataGridView1.DataSource = order.GetAll();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void FormOrders_Load(object sender, EventArgs e)
        {
            ListOrders();
        }

        private void textSearch_TextChanged(object sender, EventArgs e)
        {
            dataGridView1.DataSource = order.FilterByNameOrId(textSearch.Text);
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            panel1.Enabled = true;
            order.state = EntityState.Inserted;
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                panel1.Enabled = true;
                order.state = EntityState.Updated;
                order.Id = Convert.ToInt32(dataGridView1.CurrentRow.Cells[0].Value);
                txtName.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
                txtUnitPrice.Text = (dataGridView1.CurrentRow.Cells[2].Value.ToString());
                txtQuantity.Text = (dataGridView1.CurrentRow.Cells[3].Value.ToString());
            }
            else
            {
                MessageBox.Show("Selecciona una fila");
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                order.state = EntityState.Deleted;
                order.Id = Convert.ToInt32(dataGridView1.CurrentRow.Cells[0].Value);
                string result = order.SaveChanges();
                MessageBox.Show(result);
                ListOrders();
            }
            else
            {
                MessageBox.Show("Selecciona una fila");
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            order.ProductName = txtName.Text;
            if (txtUnitPrice.Text.Length > 0 && txtQuantity.Text.Length > 0)
            {
                if((decimal.TryParse(txtUnitPrice.Text, out _) || int.TryParse(txtUnitPrice.Text, out _)))
                {
                    if(int.TryParse(txtQuantity.Text, out _))
                    {
                        order.UnitPrice = Convert.ToDecimal(txtUnitPrice.Text);
                        order.Quantity = Convert.ToInt32(txtQuantity.Text);
                        bool valid = new DataValidations(order).Validate();
                        if (valid)
                        {
                            string result = order.SaveChanges();
                            MessageBox.Show(result);
                            ListOrders();
                            Clear();
                        }
                    }
                    else
                    {
                        MessageBox.Show("La Cantidad debe ser un número entero");
                    }
                }
                else
                {
                    MessageBox.Show("El Precio Unitario debe ser numérico");
                }
            }
            else
            {
                MessageBox.Show("Precio Unitario y Cantidad son requeridos");
            }
        }

        private void Clear()
        {
            panel1.Enabled = false;
            txtName.Clear();
            txtUnitPrice.Clear();
            txtQuantity.Clear();
        }

        private void CmbBoxMonths_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListFiltered(CmbBoxMonths.SelectedIndex);
        }

        private void ListFiltered(int month)
        {
            try
            {
                if(month != 0)
                {
                    dataGridView1.DataSource = order.FilterByMonth(month);
                }
                else
                {
                    dataGridView1.DataSource = order.GetAll();
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Clear();
        }

        private void btnFilter_Click(object sender, EventArgs e)
        {
            try
            {
                if (CmbBoxMonths.SelectedIndex != 0 && txtFilter.Text.Length > 0)
                {
                    dataGridView1.DataSource = order.FilterByMonthAndDate(CmbBoxMonths.SelectedIndex, txtFilter.Text);
                }
                else
                {
                    dataGridView1.DataSource = order.GetAll();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
