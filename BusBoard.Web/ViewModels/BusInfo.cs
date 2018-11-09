using System.Collections.Generic;
using BusBoard.Api;

namespace BusBoard.Web.ViewModels
{
  public class BusInfo
  {
    public BusInfo(string postCode)
    {
      PostCode = postCode;
    }

    public string PostCode { get; set; }

      public List<string> BusesInfo => ApiCaller.GetBusInfoPostcode(PostCode);
  }
}