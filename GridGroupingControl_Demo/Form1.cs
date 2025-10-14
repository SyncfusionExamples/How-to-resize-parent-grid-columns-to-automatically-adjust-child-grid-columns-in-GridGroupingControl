using Syncfusion.Windows.Forms.Grid;
using Syncfusion.Windows.Forms.Grid.Grouping;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Equal_size_for_all_columns
{
    public partial class Form1 : Form
    {
        private int numberParentRows = 5;
        private int numberChildRows = 20;
        public Form1()
        {
            InitializeComponent();

            //Event subscription
            gridGroupingControl1.TableModel.ColWidthsChanging += OnColWidthsChanging;
        }

        //Event customization
        private void OnColWidthsChanging(object sender, GridRowColSizeChangingEventArgs e)
        {
            int col = e.From;
            GridTable table1 = this.gridGroupingControl1.GetTable("ChildTable");
            table1.TableModel.ColWidths[col - 1] = Convert.ToInt32(e.Values.GetValue(0));            
        }

        private void OnLoad(object sender, EventArgs e)
        {
            DataTable parentTable = GetParentTable();
            DataTable childTable = GetChildTable();

            DataSet ds = new DataSet();          
            ds.Tables.AddRange(new DataTable[] { parentTable, childTable });
            DataRelation parentToChild = new DataRelation("ParentToChild", parentTable.Columns["parentID"], childTable.Columns["ParentID"]);
            ds.Relations.AddRange(new DataRelation[] { parentToChild });

            //set the datasource to the parent datatable
            this.gridGroupingControl1.DataSource = parentTable;
            GridTableDescriptor parentTD = this.gridGroupingControl1.TableDescriptor;
            GridTableDescriptor childTD = parentTD.Relations["ParentToChild"].ChildTableDescriptor;
            childTD.TableOptions.ShowRowHeader = false;

        }

        private DataTable GetParentTable()
        {
            DataTable dt = new DataTable("ParentTable");

            dt.Columns.Add(new DataColumn("parentID")); 
            dt.Columns.Add(new DataColumn("ParentName"));
            dt.Columns.Add(new DataColumn("ParentDec"));

            for (int i = 0; i < numberParentRows; ++i)
            {
                DataRow dr = dt.NewRow();
                dr[0] = i;
                dr[1] = string.Format("parentName{0}", i);
                dr[2] = string.Format("desc{0}", i);
                dt.Rows.Add(dr);
            }

            return dt;
        }

        private DataTable GetChildTable()
        {
            DataTable dt = new DataTable("ChildTable");

            dt.Columns.Add(new DataColumn("ChildID")); 
            dt.Columns.Add(new DataColumn("Name"));
            dt.Columns.Add(new DataColumn("ParentID"));
            dt.Columns.Add(new DataColumn("SalesID"));


            for (int i = 0; i < numberChildRows; ++i)
            {
                DataRow dr = dt.NewRow();
                dr[0] = i.ToString();
                dr[1] = string.Format("ChildName{0}", i);
                dr[2] = (i % numberParentRows).ToString();
                dr[3] = (i % numberParentRows).ToString();

                dt.Rows.Add(dr);
            }
            return dt;
        }
    }
}