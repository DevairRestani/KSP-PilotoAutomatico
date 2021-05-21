using KRPC.Client;
using KRPC.Client.Services.SpaceCenter;
using System;
using System.Threading;

namespace KSP_PilotoAutomatico.Manobras
{
    class DecolagemOrbital
    {
        public static void Start(Connection conn)
        {
            var vessel = conn.SpaceCenter().ActiveVessel;

            float turnStartAltitude = 250;
            float targetAltitude;
            if (vessel.Orbit.Body.AtmosphereDepth < 5000)
            {
                targetAltitude = 20000;
            }
            else
            {
                targetAltitude = vessel.Orbit.Body.AtmosphereDepth * 1.50f;
            }
            float turnEndAltitude = targetAltitude * .40f;

            // Set up streams for telemetry
            var ut = conn.AddStream(() => conn.SpaceCenter().UT);
            var flight = vessel.Flight();
            var verticalSpeed = conn.AddStream(() => vessel.Flight(vessel.Orbit.Body.ReferenceFrame).Speed);
            var altitude = conn.AddStream(() => flight.MeanAltitude);
            var apoapsis = conn.AddStream(() => vessel.Orbit.ApoapsisAltitude);

            // Pre-launch setup
            vessel.Control.SAS = false;
            vessel.Control.RCS = false;
            vessel.Control.Throttle = 1;

            Console.WriteLine("Altitude de orbita: " + targetAltitude.ToString());
            Console.WriteLine("Altitude de fim do giro: " + turnEndAltitude.ToString());

            // Countdown...
            Console.WriteLine("3...");
            Thread.Sleep(1000);
            Console.WriteLine("2...");
            Thread.Sleep(1000);
            Console.WriteLine("1...");
            Thread.Sleep(1000);
            Console.WriteLine("Lançar!");

            // Activate the first stage
            //vessel.Control.ActivateNextStage();
            vessel.AutoPilot.Engage();
            vessel.AutoPilot.TargetPitchAndHeading(90, 90);
            //vessel.AutoPilot.TargetDirection = Tuple.Create(0.0, 1.0, 0.0);

            double turnAngle = 0;
            vessel.Control.ActivateNextStage();

            while (true)
            {
                if (altitude.Get() > turnStartAltitude &&
                    altitude.Get() < turnEndAltitude)
                {
                    double frac = (altitude.Get() - turnStartAltitude)
                                  / (turnEndAltitude - turnStartAltitude);
                    double newTurnAngle = frac * 90.0;
                    if (Math.Abs(newTurnAngle - turnAngle) > 0.5)
                    {
                        turnAngle = newTurnAngle;
                        vessel.AutoPilot.TargetPitchAndHeading(
                            (float)(90 - turnAngle), 90);
                    }
                }

                // Decrease throttle when approaching target apoapsis
                if (apoapsis.Get() > targetAltitude * 0.9)
                {
                    Console.WriteLine("Approaching target apoapsis");
                    break;
                }

                //await Task.Delay(100);
            }

            Console.WriteLine("Aproximação do apoastro");

            // Disable engines when target apoapsis is reached
            vessel.Control.Throttle = 0.25f;

            while (apoapsis.Get() < targetAltitude)
            {
            }
            Console.WriteLine("Target apoapsis reached");
            vessel.Control.Throttle = 0;

            // Wait until out of atmosphere
            Console.WriteLine("Coasting out of atmosphere");
            while (altitude.Get() < vessel.Orbit.Body.AtmosphereDepth)
            {
            }

            // Plan circularization burn (using vis-viva equation)
            Console.WriteLine("Planning circularization burn");
            float mu = vessel.Orbit.Body.GravitationalParameter;
            double r = vessel.Orbit.Apoapsis;
            double a1 = vessel.Orbit.SemiMajorAxis;
            double a2 = r;
            double v1 = Math.Sqrt(mu * ((2.0 / r) - (1.0 / a1)));
            double v2 = Math.Sqrt(mu * ((2.0 / r) - (1.0 / a2)));
            double deltaV = v2 - v1;
            var node = vessel.Control.AddNode(
                ut.Get() + vessel.Orbit.TimeToApoapsis, prograde: (float)deltaV);



            Manobrar.Start(conn);
            Console.WriteLine("Lançamento Completo");

        }
    }
}
