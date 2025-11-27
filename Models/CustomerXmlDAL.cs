using System.Data;

namespace MVCDHProject.Models
***REMOVED***
    public class CustomerXmlDAL : ICustomerDAL
    ***REMOVED***
        DataSet ds;
        private readonly IWebHostEnvironment environment;

        public CustomerXmlDAL(IWebHostEnvironment environment)
        ***REMOVED***
            ds = new DataSet();
            ds.ReadXml("Customer.xml");
            ds.Tables[0].PrimaryKey = new DataColumn[] ***REMOVED*** ds.Tables[0].Columns["Custid"] ***REMOVED***;
            this.environment = environment;
      ***REMOVED***
        public List<Customer> Customers_Select()
        ***REMOVED***
            List<Customer> customers = new List<Customer>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            ***REMOVED***
                Customer obj = new Customer
                ***REMOVED***
                    Custid = Convert.ToInt32(dr["Custid"]),
                    Name = Convert.ToString(dr["Name"]),
                    Balance = Convert.ToDecimal(dr["Balance"]),
                    City = (string)dr["City"],
                    Status = Convert.ToBoolean(dr["Status"])
              ***REMOVED***;
                customers.Add(obj);
          ***REMOVED***
            return customers;
      ***REMOVED***
        public Customer Customer_Select(int Custid)
        ***REMOVED***
            DataRow? dr = ds.Tables[0].Rows.Find(Custid);
            Customer customer = new Customer
            ***REMOVED***
                Custid = Convert.ToInt32(dr["Custid"]),
                Name = Convert.ToString(dr["Name"]),
                Balance = Convert.ToDecimal(dr["Balance"]),
                City = (string)dr["City"],
                Status = Convert.ToBoolean(dr["Status"])
          ***REMOVED***;
            return customer;
      ***REMOVED***
        public void Customer_Insert(Customer customer, IFormFile photo)
        ***REMOVED***
            DataRow dr = ds.Tables[0].NewRow();
            dr["Custid"] = customer.Custid;
            dr["Name"] = customer.Name;
            dr["Balance"] = customer.Balance;
            dr["City"] = customer.City;
            dr["Status"] = customer.Status;

            ds.Tables[0].Rows.Add(dr);

            ds.WriteXml("Customer.xml");
      ***REMOVED***
        public void Customer_Update(Customer customer)
        ***REMOVED***
            DataRow? dr = ds.Tables[0].Rows.Find(customer.Custid);

            int index = ds.Tables[0].Rows.IndexOf(dr);

            ds.Tables[0].Rows[index]["Name"] = customer.Name;
            ds.Tables[0].Rows[index]["Balance"] = customer.Balance;
            ds.Tables[0].Rows[index]["City"] = customer.City;

            ds.WriteXml("Customer.xml");
      ***REMOVED***
        public void Customer_Delete(int Custid)
        ***REMOVED***
            DataRow? dr = ds.Tables[0].Rows.Find(Custid);

            int index = ds.Tables[0].Rows.IndexOf(dr);

            ds.Tables[0].Rows[index].Delete();

            ds.WriteXml("Customer.xml");
      ***REMOVED***
        public string UploadPhoto(IFormFile photo)
        ***REMOVED***
            if (photo == null || photo.Length == 0)
            ***REMOVED***
                return null;
          ***REMOVED***

            string uploadsFolder = Path.Combine(environment.WebRootPath, "images/customers");
            if (!Directory.Exists(uploadsFolder))
            ***REMOVED***
                Directory.CreateDirectory(uploadsFolder);
          ***REMOVED***

            string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(photo.FileName);
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            ***REMOVED***
                photo.CopyTo(fileStream);
          ***REMOVED***

            return "/images/customers/" + uniqueFileName;
      ***REMOVED***

        public void DeletePhotoFile(string photoPath)
        ***REMOVED***
            if (!string.IsNullOrEmpty(photoPath))
            ***REMOVED***
                string filePath = Path.Combine(environment.WebRootPath, photoPath.TrimStart('/'));
                if (File.Exists(filePath))
                ***REMOVED***
                    File.Delete(filePath);
              ***REMOVED***
          ***REMOVED***
      ***REMOVED***
  ***REMOVED***
***REMOVED***
