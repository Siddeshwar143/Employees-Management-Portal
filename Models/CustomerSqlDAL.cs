
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace MVCDHProject.Models
***REMOVED***
    public class CustomerSqlDAL : ICustomerDAL
    ***REMOVED***
        private readonly MVCCoreDbContext context;
        private readonly IWebHostEnvironment environment;

        public CustomerSqlDAL(MVCCoreDbContext context, IWebHostEnvironment environment)
        ***REMOVED***
            this.context = context;
            this.environment = environment;
      ***REMOVED***
        public List<Customer> Customers_Select()
        ***REMOVED***
            var customers = context.Customers.Where(C => C.Status == true).ToList();
            return customers;
      ***REMOVED***
        public Customer Customer_Select(int Custid)
        ***REMOVED***
            Customer customer = context.Customers.Find(Custid);
            if (customer == null)
            ***REMOVED***
                throw new Exception("No customer exist's with given Custid.");
          ***REMOVED***
            return customer;
      ***REMOVED***

        public void Customer_Insert(Customer customer, IFormFile photo)
        ***REMOVED***
            // Handle photo upload if provided
            if (photo != null && photo.Length > 0)
            ***REMOVED***
                string uploadsFolder = Path.Combine(environment.WebRootPath, "images/customers");
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                string uniqueFileName = $"***REMOVED***Guid.NewGuid()***REMOVED******REMOVED***Path.GetExtension(photo.FileName)***REMOVED***";
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                // Save the file to the server
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                ***REMOVED***
                    photo.CopyTo(fileStream);
              ***REMOVED***

                customer.photo = "/images/customers/" + uniqueFileName;
          ***REMOVED***
            else
            ***REMOVED***
                customer.photo = null; 
          ***REMOVED***

            customer.Status = true;

            context.Customers.Add(customer);
            context.SaveChanges();
      ***REMOVED***

        public void Customer_Delete(int Custid)
        ***REMOVED***
            Customer customer = context.Customers.Find(Custid);
            customer.Status = false;
            context.SaveChanges();
      ***REMOVED***

        public void Customer_Update(Customer customer)
        ***REMOVED***
            var existingCustomer = context.Customers.Find(customer.Custid);
            customer.Status = true;
            if (existingCustomer == null)
            ***REMOVED***
                throw new Exception("Customer not found");
          ***REMOVED***

            context.Entry(existingCustomer).CurrentValues.SetValues(customer);
            context.SaveChanges();
      ***REMOVED***

        public string UploadPhoto(IFormFile photo)
        ***REMOVED***
            if (photo == null || photo.Length == 0)
                return null;

            string uploadsFolder = Path.Combine(environment.WebRootPath, "images/customers");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            string uniqueFileName = $"***REMOVED***Guid.NewGuid()***REMOVED******REMOVED***Path.GetExtension(photo.FileName)***REMOVED***";
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            ***REMOVED***
                photo.CopyTo(fileStream);
          ***REMOVED***

            return $"/images/customers/***REMOVED***uniqueFileName***REMOVED***";
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
