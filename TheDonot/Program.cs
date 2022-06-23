using System;

namespace TheDonot
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            Console.SetWindowSize(80,25);
            const int screenWitdt = 80;
            const int screenhigth = 25;
            
            const float thetaSpaceing= (float) 0.007;
            const float phiSpaceing = (float) 0.002;

            const float r1 = 1;
            const float r2 = 2;
            const float k2 = 5;
            
            //todo add values to k1, k2 and z 
            //K1 and K2 can be tweaked together to change the field of view and flatten
            //or exaggerate the depth of the object.
            
            // Calculate K1 based on screen size: the maximum x-distance occurs
            // roughly at the edge of the torus, which is at x=R1+R2, z=0.  we
            // want that to be displaced 3/8ths of the width of the screen, which
            // is 3/4th of the way from the center to the side of the screen.
            // screen_width*3/8 = K1*(R1+R2)/(K2+0)
            // screen_width*K2*3/(8*(R1+R2)) = K1
            

            //static void RenderFrame(float a, float b)
            //{
            const float a = 0;
            const float b = 0;

            float cosA = (float) Math.Cos(a),sinA = (float) Math.Sin(a);
                float cosB = (float) Math.Cos(b),sinB = (float) Math.Cos(b);

                Range[] output = {0..^screenhigth,0..^screenWitdt};
                
                Range[] zbuffer = {0..^screenhigth,0..^screenWitdt};
                // int  = Convert.ToInt32(oldzbuffer);
                //TODO HDHDHD

               for (float theta = 0; theta < 2 * Math.PI; theta += thetaSpaceing)
               {
                   float costheta = (float) Math.Cos(theta),sintheta = (float) Math.Sin(theta);

                   for (float phi = 0; phi < 2*Math.PI;phi +=phiSpaceing)
                   {
                       float cosphi = (float) Math.Cos(phi), sinphi = (float) Math.Sin(phi);

                       var circlex = r2 + r1 * costheta;
                       var circley = r1 * sintheta;

                       var x = circlex * (cosB * cosphi + sinA * sinB * sinphi) - circley * cosA * cosB;
                       var y = circlex * (sinB * cosphi - sinA * cosB * sinphi) + circley * cosA * cosB;
                       var z = (int)Math.Round(k2 + cosA * circlex * sinphi + circley * sinA);
                       var ooz = 1 / z;

                       Index xp = (int)Math.Round(screenWitdt / 2 + k1 * ooz * x);
                       Index yp = (int)Math.Round(screenhigth / 2 + k1 * ooz * y);
                       //Console.Write(yp);
                       //Console.Write(xp);
                        
                       var l = cosphi*costheta*sinB - cosA*costheta*sinphi -
                           sinA*sintheta + cosB*(cosA*sintheta - costheta*sinA*sinphi);

                       if (!(l > 0)) continue;
                       if (ooz < zbuffer[xp] && ooz > zbuffer[yp]) continue;
                       zbuffer[xp,yp] = ooz;

                       var lIndex = (int) (l*8);
                       // luminance_index is now in the range 0..11 (8*sqrt(2) = 11.3)
                       // now we lookup the character corresponding to the
                       // luminance and plot it in our output:
                       output[yp] = ".,-~:;=!*#$@"[lIndex < 0 ? lIndex : 0];

                   }
               }
               for (Int32 j = 0; j < screenhigth; j++)
               {
                   Console.WriteLine(output[j]);

                   for (Int32 i = 0; i < screenWitdt; i++) {
                       Console.WriteLine(output[i]);
                       
                   }
                   Console.WriteLine('\n'); 
               }
               
              
            
        }
    }
}

