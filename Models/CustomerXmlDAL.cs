using System.Data;

namespace MVCDHProject.Models
{
    public class CustomerXmlDAL : ICustomerDAL
    {
        DataSet ds;
        private readonly IWebHostEnvironment environment;

        public CustomerXmlDAL(IWebHostEnvironment environment)
        {
            ds = new DataSet();
            ds.ReadXml("Customer.xml");
            ds.Tables[0].PrimaryKey = new DataColumn[] { ds.Tables[0].Columns["Custid"] };
            this.environment = environment;
        }
        public List<Customer> Customers_Select()
        {
            List<Customer> customers = new List<Customer>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                Customer obj = new Customer
                {
                    Custid = Convert.ToInt32(dr["Custid"]),
                    Name = Convert.ToString(dr["Name"]),
                    Balance = Convert.ToDecimal(dr["Balance"]),
                    City = (string)dr["City"],
                    Status = Convert.ToBoolean(dr["Status"])
                };
                customers.Add(obj);
            }
            return customers;
        }
        public Customer Customer_Select(int Custid)
        {
            DataRow? dr = ds.Tables[0].Rows.Find(Custid);
            Customer customer = new Customer
            {
                Custid = Convert.ToInt32(dr["Custid"]),
                Name = Convert.ToString(dr["Name"]),
                Balance = Convert.ToDecimal(dr["Balance"]),
                City = (string)dr["City"],
                Status = Convert.ToBoolean(dr["Status"])
            };
            return customer;
        }
        public void Customer_Insert(Customer customer)
        {
            DataRow dr = ds.Tables[0].NewRow();
            dr["Custid"] = customer.Custid;
            dr["Name"] = customer.Name;
            dr["Balance"] = customer.Balance;
            dr["City"] = customer.City;
            dr["Status"] = customer.Status;

            ds.Tables[0].Rows.Add(dr);

            ds.WriteXml("Customer.xml");
        }
        public void Customer_Update(Customer customer)
        {
            DataRow? dr = ds.Tables[0].Rows.Find(customer.Custid);

            int index = ds.Tables[0].Rows.IndexOf(dr);

            ds.Tables[0].Rows[index]["Name"] = customer.Name;
            ds.Tables[0].Rows[index]["Balance"] = customer.Balance;
            ds.Tables[0].Rows[index]["City"] = customer.City;

            ds.WriteXml("Customer.xml");
        }
        public void Customer_Delete(int Custid)
        {
            DataRow? dr = ds.Tables[0].Rows.Find(Custid);

            int index = ds.Tables[0].Rows.IndexOf(dr);

            ds.Tables[0].Rows[index].Delete();

            ds.WriteXml("Customer.xml");
        }
        public string UploadPhoto(IFormFile photo)
        {
            if (photo == null || photo.Length == 0)
            {
                return null;
            }

            string uploadsFolder = Path.Combine(environment.WebRootPath, "images/customers");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(photo.FileName);
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                photo.CopyTo(fileStream);
            }

            return "/images/customers/" + uniqueFileName;
        }

        public void DeletePhotoFile(string photoPath)
        {
            if (!string.IsNullOrEmpty(photoPath))
            {
                string filePath = Path.Combine(environment.WebRootPath, photoPath.TrimStart('/'));
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
            }
        }
    }
}
