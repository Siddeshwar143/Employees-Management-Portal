using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVCDHProject.Models;
using System;
using Microsoft.AspNetCore.Hosting;

namespace MVCDHProject.Controllers
***REMOVED***
    [Authorize]
    public class CustomerController : Controller
    ***REMOVED***
        private readonly ICustomerDAL _dal;
        private readonly IWebHostEnvironment _environment;


        public CustomerController(ICustomerDAL dal, IWebHostEnvironment environment)
        ***REMOVED***
            _dal = dal ?? throw new ArgumentNullException(nameof(dal));
            _environment = environment ?? throw new ArgumentNullException(nameof(environment));
      ***REMOVED***

        [AllowAnonymous]
        public ViewResult DisplayCustomers()
        ***REMOVED***
            var customers = _dal.Customers_Select();
            return View(customers);
      ***REMOVED***

        [AllowAnonymous]
        public IActionResult DisplayCustomer(int custid)
        ***REMOVED***
            try
            ***REMOVED***
                var customer = _dal.Customer_Select(custid);
                if (customer == null)
                ***REMOVED***
                    return NotFound();
              ***REMOVED***
                return View(customer);
          ***REMOVED***
            catch (Exception ex)
            ***REMOVED***
                // Log the exception here
                return View("Error", new ErrorViewModel ***REMOVED*** RequestId = ex.Message ***REMOVED***);
          ***REMOVED***
      ***REMOVED***

        [HttpGet]
        public ViewResult AddCustomer()
        ***REMOVED***

            return View(new Customer());
      ***REMOVED***

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddCustomer(Customer customer, IFormFile photo)
        ***REMOVED***
            try
            ***REMOVED***
                if (ModelState.IsValid)
                ***REMOVED***
                    // Call DAL method with both customer and photo parameters
                    _dal.Customer_Insert(customer, photo);

                    TempData["SuccessMessage"] = "Customer added successfully!";
                    return RedirectToAction(nameof(DisplayCustomers));
              ***REMOVED***

                // If model state is invalid, return to the form with errors
                return View(customer);
          ***REMOVED***
            catch (Exception ex)
            ***REMOVED***
                ModelState.AddModelError("", "Error creating customer: " + ex.Message);
                return View(customer);
          ***REMOVED***
      ***REMOVED***

        [HttpGet]
        public IActionResult EditCustomer(int custid)
        ***REMOVED***
            try
            ***REMOVED***
                var customer = _dal.Customer_Select(custid);
                if (customer == null)
                ***REMOVED***
                    return NotFound();
              ***REMOVED***
                return View(customer);
          ***REMOVED***
            catch (Exception ex)
            ***REMOVED***
                TempData["ErrorMessage"] = $"Error loading customer: ***REMOVED***ex.Message***REMOVED***";
                return RedirectToAction(nameof(DisplayCustomers));
          ***REMOVED***
      ***REMOVED***

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditCustomer(Customer customer, IFormFile photo, bool deletePhoto = false)
        ***REMOVED***
            if (ModelState.IsValid)
            ***REMOVED***
                try
                ***REMOVED***
                    var existingCustomer = _dal.Customer_Select(customer.Custid);
                    customer.Status = true;
                    if (existingCustomer == null)
                    ***REMOVED***
                        return NotFound();
                  ***REMOVED***

                    // Handle photo deletion
                    if (deletePhoto && !string.IsNullOrEmpty(existingCustomer.photo))
                    ***REMOVED***
                        _dal.DeletePhotoFile(existingCustomer.photo);
                        customer.photo = null;
                  ***REMOVED***
                    // Handle new photo upload
                    else if (photo != null && photo.Length > 0)
                    ***REMOVED***
                        // Delete old photo if exists
                        if (!string.IsNullOrEmpty(existingCustomer.photo))
                        ***REMOVED***
                            _dal.DeletePhotoFile(existingCustomer.photo);
                      ***REMOVED***

                        // Upload new photo
                        customer.photo = _dal.UploadPhoto(photo);
                  ***REMOVED***
                    else
                    ***REMOVED***
                        // Keep existing photo if no changes
                        customer.photo = existingCustomer.photo;
                  ***REMOVED***

                    // Update other customer properties
                    existingCustomer.Name = customer.Name;
                    existingCustomer.Balance = customer.Balance;
                    existingCustomer.City = customer.City;
                    existingCustomer.Status = customer.Status;
                    existingCustomer.photo = customer.photo;

                    _dal.Customer_Update(existingCustomer);

                    TempData["SuccessMessage"] = "Customer updated successfully!";
                    return RedirectToAction(nameof(DisplayCustomers));
              ***REMOVED***
                catch (Exception ex)
                ***REMOVED***
                    ModelState.AddModelError("", $"Error updating customer: ***REMOVED***ex.Message***REMOVED***");
              ***REMOVED***
          ***REMOVED***

            return View(customer);
      ***REMOVED***

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePhoto(int id)
        ***REMOVED***
            try
            ***REMOVED***
                var customer = _dal.Customer_Select(id);
                if (customer == null)
                ***REMOVED***
                    return NotFound();
              ***REMOVED***

                if (!string.IsNullOrEmpty(customer.photo))
                ***REMOVED***
                    _dal.DeletePhotoFile(customer.photo);
                    customer.photo = null;
                    _dal.Customer_Update(customer);

                    TempData["SuccessMessage"] = "Photo deleted successfully!";
              ***REMOVED***

                return RedirectToAction(nameof(EditCustomer), new ***REMOVED*** custid = id ***REMOVED***);
          ***REMOVED***
            catch (Exception ex)
            ***REMOVED***
                TempData["ErrorMessage"] = $"Error deleting photo: ***REMOVED***ex.Message***REMOVED***";
                return RedirectToAction(nameof(EditCustomer), new ***REMOVED*** custid = id ***REMOVED***);
          ***REMOVED***
      ***REMOVED***
        public RedirectToActionResult DeleteCustomer(int Custid)
        ***REMOVED***
            _dal.Customer_Delete(Custid);
            return RedirectToAction("DisplayCustomers");
      ***REMOVED***
  ***REMOVED***
***REMOVED***
