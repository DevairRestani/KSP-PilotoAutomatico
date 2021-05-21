using KRPC.Client;
using KRPC.Client.Services.SpaceCenter;
using System;
using System.Threading;

namespace KSP_PilotoAutomatico.Manobras
{
    class Pouso
    {
        public static void Start(Connection conn)
        {
            var vessel = conn.SpaceCenter().ActiveVessel;
            var gravidade = vessel.Orbit.Body.SurfaceGravity;
            var massaTotal = conn.AddStream(() => vessel.Mass);
            var voo = vessel.Flight(vessel.Orbit.Body.ReferenceFrame);
            var velocidadeVertical = conn.AddStream(() => voo.VerticalSpeed);
            var retrogrado = conn.AddStream(() => voo.Retrograde);

            Lancar(vessel);

            vessel.AutoPilot.Engage();

            while (true)
            {
                var reference = ReferenceFrame.CreateRelative(conn, vessel.AutoPilot.ReferenceFrame, retrogrado.Get());
                vessel.AutoPilot.ReferenceFrame = reference;
            }

            conn.Dispose();
            Console.WriteLine("Fim");
        }

        private static void Lancar(Vessel vessel)
        {
            vessel.AutoPilot.Engage();

            vessel.AutoPilot.TargetPitchAndHeading(90, 90);
            vessel.Control.Throttle = 1;
            vessel.Control.ActivateNextStage();

            while (vessel.Orbit.Apoapsis < 700)
            {
                Thread.Sleep(100);
            }

            vessel.AutoPilot.TargetPitchAndHeading(0, 0);

            while (vessel.Orbit.Apoapsis < 7000)
            {
                Thread.Sleep(100);
            }

            vessel.Control.Throttle = 0;

            vessel.AutoPilot.Disengage();
        }
    }
}
