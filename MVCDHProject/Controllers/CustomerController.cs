using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVCDHProject.Models;
using System;
using Microsoft.AspNetCore.Hosting;

namespace MVCDHProject.Controllers
{
    [Authorize]
    public class CustomerController : Controller
    {
        private readonly ICustomerDAL _dal;
        private readonly IWebHostEnvironment _environment;


        public CustomerController(ICustomerDAL dal, IWebHostEnvironment environment)
        {
            _dal = dal ?? throw new ArgumentNullException(nameof(dal));
            _environment = environment ?? throw new ArgumentNullException(nameof(environment));
        }

        [AllowAnonymous]
        public ViewResult DisplayCustomers()
        {
            var customers = _dal.Customers_Select();
            return View(customers);
        }

        [AllowAnonymous]
        public IActionResult DisplayCustomer(int custid)
        {
            try
            {
                var customer = _dal.Customer_Select(custid);
                if (customer == null)
                {
                    return NotFound();
                }
                return View(customer);
            }
            catch (Exception ex)
            {
                // Log the exception here
                return View("Error", new ErrorViewModel { RequestId = ex.Message });
            }
        }

        [HttpGet]
        public ViewResult AddCustomer()
        {

            return View(new Customer());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddCustomer(Customer customer, IFormFile photo)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Call DAL method with both customer and photo parameters
                    _dal.Customer_Insert(customer, photo);

                    TempData["SuccessMessage"] = "Customer added successfully!";
                    return RedirectToAction(nameof(DisplayCustomers));
                }

                // If model state is invalid, return to the form with errors
                return View(customer);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error creating customer: " + ex.Message);
                return View(customer);
            }
        }

        [HttpGet]
        public IActionResult EditCustomer(int custid)
        {
            try
            {
                var customer = _dal.Customer_Select(custid);
                if (customer == null)
                {
                    return NotFound();
                }
                return View(customer);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error loading customer: {ex.Message}";
                return RedirectToAction(nameof(DisplayCustomers));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditCustomer(Customer customer, IFormFile photo, bool deletePhoto = false)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var existingCustomer = _dal.Customer_Select(customer.Custid);
                    customer.Status = true;
                    if (existingCustomer == null)
                    {
                        return NotFound();
                    }

                    // Handle photo deletion
                    if (deletePhoto && !string.IsNullOrEmpty(existingCustomer.photo))
                    {
                        _dal.DeletePhotoFile(existingCustomer.photo);
                        customer.photo = null;
                    }
                    // Handle new photo upload
                    else if (photo != null && photo.Length > 0)
                    {
                        // Delete old photo if exists
                        if (!string.IsNullOrEmpty(existingCustomer.photo))
                        {
                            _dal.DeletePhotoFile(existingCustomer.photo);
                        }

                        // Upload new photo
                        customer.photo = _dal.UploadPhoto(photo);
                    }
                    else
                    {
                        // Keep existing photo if no changes
                        customer.photo = existingCustomer.photo;
                    }

                    // Update other customer properties
                    existingCustomer.Name = customer.Name;
                    existingCustomer.Balance = customer.Balance;
                    existingCustomer.City = customer.City;
                    existingCustomer.Status = customer.Status;
                    existingCustomer.photo = customer.photo;

                    _dal.Customer_Update(existingCustomer);

                    TempData["SuccessMessage"] = "Customer updated successfully!";
                    return RedirectToAction(nameof(DisplayCustomers));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Error updating customer: {ex.Message}");
                }
            }

            return View(customer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePhoto(int id)
        {
            try
            {
                var customer = _dal.Customer_Select(id);
                if (customer == null)
                {
                    return NotFound();
                }

                if (!string.IsNullOrEmpty(customer.photo))
                {
                    _dal.DeletePhotoFile(customer.photo);
                    customer.photo = null;
                    _dal.Customer_Update(customer);

                    TempData["SuccessMessage"] = "Photo deleted successfully!";
                }

                return RedirectToAction(nameof(EditCustomer), new { custid = id });
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error deleting photo: {ex.Message}";
                return RedirectToAction(nameof(EditCustomer), new { custid = id });
            }
        }
        public RedirectToActionResult DeleteCustomer(int Custid)
        {
            _dal.Customer_Delete(Custid);
            return RedirectToAction("DisplayCustomers");
        }
    }
}
