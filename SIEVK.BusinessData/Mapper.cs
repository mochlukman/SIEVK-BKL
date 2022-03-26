using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace SIEVK.BusinessData
{
    public class Mapper
    {
        /*Datatable to Object
         * contoh:
            DataTable dt = GetTable();
            Foo foo = BindData<Foo>(dt);
         */
        public T BindData<T>(DataTable dt)
        {
            // Get all columns' name
            List<string> columns = new List<string>();
            foreach (DataColumn dc in dt.Columns)
            {
                columns.Add(dc.ColumnName);
            }

            // Create object
            var ob = Activator.CreateInstance<T>();

            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                // Get all fields
                var fields = typeof(T).GetFields();
                foreach (var fieldInfo in fields)
                {
                    if (columns.Contains(fieldInfo.Name))
                    {
                        // Fill the data into the field
                        fieldInfo.SetValue(ob, dr[fieldInfo.Name]);
                    }
                }

                // Get all properties
                var properties = typeof(T).GetProperties();
                foreach (var propertyInfo in properties)
                {
                    if (columns.Contains(propertyInfo.Name))
                    {
                        // Fill the data into the property
                        if (dr[propertyInfo.Name] != DBNull.Value) //tambahan SLA
                        {
                            propertyInfo.SetValue(ob, dr[propertyInfo.Name]);
                        }
                    }
                }
            }
            return ob;
        }

        /*Datatable to List Object
         * contoh:
           List<Foo> lst = BindDataList<Foo>(dt);
         */

        public List<T> BindDataList<T>(DataTable dt)
        {
            List<string> columns = new List<string>();
            foreach (DataColumn dc in dt.Columns)
            {
                columns.Add(dc.ColumnName);
            }

            var fields = typeof(T).GetFields();
            var properties = typeof(T).GetProperties();

            List<T> lst = new List<T>();

            foreach (DataRow dr in dt.Rows)
            {
                var ob = Activator.CreateInstance<T>();

                foreach (var fieldInfo in fields)
                {
                    if (columns.Contains(fieldInfo.Name))
                    {
                        fieldInfo.SetValue(ob, dr[fieldInfo.Name]);
                    }
                }

                foreach (var propertyInfo in properties)
                {
                    if (columns.Contains(propertyInfo.Name))
                    {
                        if (dr[propertyInfo.Name] != DBNull.Value) //tambahan SLA
                        {
                            propertyInfo.SetValue(ob, dr[propertyInfo.Name]);
                        }
                    }
                }

                lst.Add(ob);
            }

            return lst;
        }

    }
}
