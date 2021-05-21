using KRPC.Client;
using KRPC.Client.Services.SpaceCenter;
using System;

namespace KSP_PilotoAutomatico.Manobras
{
    public class Manobrar
    {
        public static void Start(Connection conn)
        {
            var vessel = conn.SpaceCenter().ActiveVessel;
            var ut = conn.AddStream(() => conn.SpaceCenter().UT);
            var node = vessel.Control.Nodes[0];

            vessel.AutoPilot.Engage();

            Console.WriteLine("Orientando nave");
            vessel.AutoPilot.ReferenceFrame = node.ReferenceFrame;
            vessel.AutoPilot.TargetDirection = Tuple.Create(0.0, 1.0, 0.0);
            vessel.AutoPilot.Wait();

            // Wait until burn
            Console.WriteLine("Esperando até a queima");
            double burnUT = ut.Get() + node.TimeTo;
            conn.SpaceCenter().WarpTo(burnUT - 15);

            Console.WriteLine("Orientando nave após o warp");
            vessel.AutoPilot.ReferenceFrame = node.ReferenceFrame;
            vessel.AutoPilot.TargetDirection = Tuple.Create(0.0, 1.0, 0.0);
            vessel.AutoPilot.Wait();

            while (node.TimeTo > 0) { }

            Console.WriteLine("Executnado queima");
            if (node.RemainingDeltaV < 20)
            {
                vessel.Control.Throttle = 0.2f;
            }
            else
            {
                vessel.Control.Throttle = 1f;
            }
            while (node.RemainingDeltaV > 0.1)
            {
                if (node.RemainingDeltaV < 5.0)
                {
                    vessel.Control.Throttle = 0.1f;
                }
            }

            vessel.Control.Throttle = 0;

            Console.WriteLine("DeltaV restante: " + node.RemainingDeltaV);
            Console.WriteLine("Fim");
        }
    }
}
