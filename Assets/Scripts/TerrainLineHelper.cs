using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainLineHelper : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

public class Line
{
    private float x0, y0, x1, y1;

    public Line(float x0, float y0, float x1, float y1)
    {
        this.x0 = x0;
        this.y0 = y0;
        this.y1 = y1;
        this.x1 = x1;
    }

    private void plot(float[,,] bitmap, float x, float y, float c)
    {
        int alpha = (int)(c * 255);
        if (alpha > 255) alpha = 255;
        if (alpha < 0) alpha = 0;

        if(x > bitmap.GetLength(0) - 1 || y > bitmap.GetLength(1) - 1)
        {
            Debug.LogWarning("Trying to plot out of bounds, this is a bug");
            return;
        }

        int bottomX = ipart(x);
        int topX = bottomX + 1;
        int bottomY = ipart(y);
        int topY = bottomY + 1;
        
        float fX = fpart(x);
        float fY = fpart(y);

        // at the top, use the full value, at the bottom use 1-full
        float topLeft = Average(1 - fX, fY) * c;
        bitmap[bottomX, topY, 0] = Mathf.Max(bitmap[bottomX, topY, 0], topLeft);

        float bottomLeft = Average(1 - fX, 1 - fY);
        bitmap[bottomX, bottomY, 0] = Mathf.Max(bitmap[bottomX, bottomY, 0], bottomLeft);

        float bottomRight = Average(fX, 1 - fY);
        bitmap[topX, bottomY, 0] = Mathf.Max(bitmap[topX, bottomY, 0], bottomRight);
        
        float topRight = Average(fX, fY);
        bitmap[topX, topY, 0] = Mathf.Max(bitmap[topX, topY, 0], topRight);
    }

    private float Average(float f1, float f2)
    {
        return (f1 + f2) / 2f;
    }

    int ipart(float x) { return (int)x;}

    int round(float x) {return ipart(x+0.5f);}

    float fpart(float x) {
        if(x<0) return (1-(x-Mathf.Floor(x)));
        return (x-Mathf.Floor(x));
    }

    float rfpart(float x) {
        return 1-fpart(x);
    }

    public void draw(float[,,] bitmap) {
        bool steep = Mathf.Abs(y1-y0) > Mathf.Abs(x1-x0);
        float temp;
        if(steep){
            temp=x0; x0=y0; y0=temp;
            temp=x1;x1=y1;y1=temp;
        }
        if(x0>x1){
            temp = x0;x0=x1;x1=temp;
            temp = y0;y0=y1;y1=temp;
        }

        float dx = x1-x0;
        float dy = y1-y0;
        float gradient = dy/dx;

        float xEnd = round(x0);
        float yEnd = y0+gradient*(xEnd-x0);
        float xGap = rfpart(x0+0.5f);
        float xPixel1 = xEnd;
        float yPixel1 = ipart(yEnd);

        if(steep){
            plot(bitmap, yPixel1,   xPixel1, rfpart(yEnd)*xGap);
            plot(bitmap, yPixel1+1, xPixel1,  fpart(yEnd)*xGap);
        }else{
            plot(bitmap, xPixel1,yPixel1, rfpart(yEnd)*xGap);
            plot(bitmap, xPixel1, yPixel1+1, fpart(yEnd)*xGap);
        }
        float intery = yEnd+gradient;

        xEnd = round(x1);
        yEnd = y1+gradient*(xEnd-x1);
        xGap = fpart(x1+0.5f);
        float xPixel2 = xEnd;
        float yPixel2 = ipart(yEnd);
        if(steep){
            plot(bitmap, yPixel2,   xPixel2, rfpart(yEnd)*xGap);
            plot(bitmap, yPixel2+1, xPixel2, fpart(yEnd)*xGap);
        }else{
            plot(bitmap, xPixel2, yPixel2, rfpart(yEnd)*xGap);
            plot(bitmap, xPixel2, yPixel2+1, fpart(yEnd)*xGap);
        }

        if(steep){
            for(int x=(int)(xPixel1+1);x<=xPixel2-1;x++){
                plot(bitmap, ipart(intery), x, rfpart(intery));
                plot(bitmap, ipart(intery)+1, x, fpart(intery));
                intery+=gradient;
            }
        }else{
            for(int x=(int)(xPixel1+1);x<=xPixel2-1;x++){
                plot(bitmap, x,ipart(intery), rfpart(intery));
                plot(bitmap, x, ipart(intery)+1, fpart(intery));
                intery+=gradient;
            }
        }
    }
}
