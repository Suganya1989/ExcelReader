using BusinessFacade;
using Simple.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simple.Data.Ado;

namespace ServiceCodeDataAccess
{
    public class ServiceCodeCommand
    {
        public void InsertServiceCodes(ServiceCodes serviceCodes)
        {
            var db = Database.OpenNamedConnection("PAPConnection");
            dynamic val = db.SERVICE_CODES.Insert(SERVICE_CODE: serviceCodes.ServiceCode, SERVICE_CODE_ID: serviceCodes.ServiceCodeID, EDI_CODE: serviceCodes.EDICode, SVCE_CODE_GP_ID: serviceCodes.SvceCodeGPID);

        }
    }
}
