
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace MVCDHProject.Models
{
    public class CustomerSqlDAL : ICustomerDAL
    {
        private readonly MVCCoreDbContext context;
        private readonly IWebHostEnvironment environment;

        public CustomerSqlDAL(MVCCoreDbContext context, IWebHostEnvironment environment)
        {
            this.context = context;
            this.environment = environment;
        }
        public List<Customer> Customers_Select()
        {
            var customers = context.Customers.Where(C => C.Status == true).ToList();
            return customers;
        }
        public Customer Customer_Select(int Custid)
        {
            Customer customer = context.Customers.Find(Custid);
            if (customer == null)
            {
                throw new Exception("No customer exist's with given Custid.");
            }
            return customer;
        }

        public void Customer_Insert(Customer customer, IFormFile photo)
        {
            // Handle photo upload if provided
            if (photo != null && photo.Length > 0)
            {
                string uploadsFolder = Path.Combine(environment.WebRootPath, "images/customers");
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                string uniqueFileName = $"{Guid.NewGuid()}{Path.GetExtension(photo.FileName)}";
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                // Save the file to the server
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    photo.CopyTo(fileStream);
                }

                customer.photo = "/images/customers/" + uniqueFileName;
            }
            else
            {
                customer.photo = null; 
            }

            customer.Status = true;

            context.Customers.Add(customer);
            context.SaveChanges();
        }

        public void Customer_Delete(int Custid)
        {
            Customer customer = context.Customers.Find(Custid);
            customer.Status = false;
            context.SaveChanges();
        }

        public void Customer_Update(Customer customer)
        {
            var existingCustomer = context.Customers.Find(customer.Custid);
            customer.Status = true;
            if (existingCustomer == null)
            {
                throw new Exception("Customer not found");
            }

            context.Entry(existingCustomer).CurrentValues.SetValues(customer);
            context.SaveChanges();
        }

        public string UploadPhoto(IFormFile photo)
        {
            if (photo == null || photo.Length == 0)
                return null;

            string uploadsFolder = Path.Combine(environment.WebRootPath, "images/customers");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            string uniqueFileName = $"{Guid.NewGuid()}{Path.GetExtension(photo.FileName)}";
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                photo.CopyTo(fileStream);
            }

            return $"/images/customers/{uniqueFileName}";
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
