using Microsoft.AspNetCore.Mvc;
using SecureDataSharingApi.Models;

namespace SecureDataSharingApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataSharingController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        // Simulated data store with realistic entries
        private static readonly List<SharedData> _dataStore = new()
        {
            // Admin-only data (sensitive, confidential)
            new SharedData 
            { 
                Id = 1, 
                Name = "Employee_Payroll_2025", 
                Content = "Confidential payroll data for Q1 2025: John Doe - $85,000, Jane Smith - $92,000", 
                AccessLevel = "Admin" 
            },
            new SharedData 
            { 
                Id = 2, 
                Name = "Security_Audit_Report", 
                Content = "Internal audit findings: 3 vulnerabilities detected in server room access control", 
                AccessLevel = "Admin" 
            },
            new SharedData 
            { 
                Id = 3, 
                Name = "Executive_Meeting_Notes", 
                Content = "Minutes from 04/07/2025: Discussed merger with XYZ Corp, budget approved at $5M", 
                AccessLevel = "Admin" 
            },
            // User-accessible data (public or less sensitive)
            new SharedData 
            { 
                Id = 4, 
                Name = "Company_Newsletter_April2025", 
                Content = "Welcome to our April 2025 newsletter! New office opening in Chicago next month.", 
                AccessLevel = "User" 
            },
            new SharedData 
            { 
                Id = 5, 
                Name = "Employee_Handbook", 
                Content = "Updated policies: Remote work allowed 3 days/week, vacation days increased to 20/year", 
                AccessLevel = "User" 
            },
            new SharedData 
            { 
                Id = 6, 
                Name = "Holiday_Schedule_2025", 
                Content = "Office closed: Jan 1, Jul 4, Nov 27-28, Dec 24-25", 
                AccessLevel = "User" 
            },
            // Developer-accessible data (technical, system-related)
            new SharedData 
            { 
                Id = 7, 
                Name = "API_Endpoint_Docs", 
                Content = "GET /api/DataSharing/{id} - Requires X-Api-Key header, returns JSON data", 
                AccessLevel = "Developer" 
            },
            new SharedData 
            { 
                Id = 8, 
                Name = "Server_Log_04-07-2025", 
                Content = "Error 500 at 14:32: Database timeout; resolved by restarting service", 
                AccessLevel = "Developer" 
            },
            new SharedData 
            { 
                Id = 9, 
                Name = "Deployment_Script", 
                Content = "Steps: Build with 'dotnet publish', deploy to Azure App Service, set env vars", 
                AccessLevel = "Developer" 
            }
        };

        public DataSharingController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("{id}")]
        public IActionResult GetData(int id, [FromQuery] string role)
        {
            // Validate role
            var adminRole = _configuration["ApiSettings:AdminRole"];
            var userRole = _configuration["ApiSettings:UserRole"];
            var developerRole = _configuration["ApiSettings:DeveloperRole"];
            if (role != adminRole && role != userRole && role != developerRole)
            {
                return BadRequest("Invalid role specified.");
            }

            var data = _dataStore.FirstOrDefault(d => d.Id == id);
            if (data == null)
            {
                return NotFound("Data not found.");
            }

            // Check access level
            if (data.AccessLevel != role)
            {
                return StatusCode(403, $"Access denied. {data.AccessLevel} role required.");
            }

            return Ok(data);
        }
    }
}