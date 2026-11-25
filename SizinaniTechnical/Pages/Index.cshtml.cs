using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using SizinaniTechnical.Data;
using SizinaniTechnical.Model;

namespace SizinaniTechnical.Pages
{
    public class IndexModel : PageModel
    {

        private readonly VehicleRepository _vehicleRepo;

        public IndexModel(VehicleRepository vehicleRepository)
        {
            _vehicleRepo = vehicleRepository;
        }

        [BindProperty]
        public RegisteredVehicle Input { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public int? ContractorID { get; set; }

        [TempData]
        public string? Message { get; set; }

        public SelectList? Contractors { get; set; }

        public List<RegisteredVehicle> Rows { get; set; }
        public List<RegisteredVehicle> Vehicles { get; set; } = new();
        public void OnGet()
        {
            Contractors = new SelectList(_vehicleRepo.GetContractors(),
                                     "ContractorID", "ContractorName", ContractorID);

            if (ContractorID.HasValue)
                Vehicles = _vehicleRepo.GetVehiclesByContractor(ContractorID.Value);
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                 _vehicleRepo.SaveRegisteredVehicle(Input);
                Message = "Registered Vehicle";
                return RedirectToPage();

            }
            catch (SqlException ex)
            {
                ModelState.AddModelError(string.Empty, $"Database error: {ex.Message}");
                return Page();
            }
        }
    }
}
