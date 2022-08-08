using System;

public class ControllerRepository
{
    private readonly IController daoBms = new ControllerDaoBms();
    private readonly IController daoInf = new ControllerDaoInf();
    
    public IController GetController(ControllerType type)
    {
        switch (type)
        {
            case ControllerType.DAO_BMS:
                return daoBms;
            case ControllerType.DAO_INF:
                return daoInf;
            default:
                throw new ArgumentException();
        }
    }
}
