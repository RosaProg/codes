using PCHI.WcfServices.PMS.Models;
using System.ServiceModel;

namespace PCHI.WcfServices.PMS.Contracts
{
    [ServiceContract]
    public interface IPatient
    {
        [OperationContract]
        Message CreatePatientAppointment(PatientAppointment patient);
    }
}
