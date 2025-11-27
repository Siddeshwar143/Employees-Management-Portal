namespace MVCDHProject.Models
{
    public interface ICustomerDAL
    {
        List<Customer> Customers_Select();
        Customer Customer_Select(int Custid);
        void Customer_Insert(Customer customer, IFormFile photo);
        void Customer_Update(Customer customer);
        void Customer_Delete(int Custid);
        string UploadPhoto(IFormFile photo);
        void DeletePhotoFile(string photoPath);

    }
}
