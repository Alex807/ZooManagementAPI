namespace backend.Enums
{
    // Defines the different roles available in the system
    // Admin: Full system access
    // Zookeeper: Manages animals and feeding schedules
    // Veterinarian: Handles medical records
    // Visitor: Read-only access
    public enum RoleName
    {
        Admin,
        Zookeeper,
        Veterinarian, 
        Visitor    
    }
}