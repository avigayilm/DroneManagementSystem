using System;

public static class Bonus
{
    //function receives coordinates 
    public static string DecimalToSexagesimal(double coord, char latOrLot)
    {
        char direction;// funciton receives char to decide wheter it is t=latitude and n=longitude
        if (latOrLot == 't')// if latitude
            if (coord >= 0)//determines how many minutse norht or south 0 is the equator  larger then 0 is north smaller is south
                direction = 'E';
            else
            {
                direction = 'W';
                coord *= -1;
            }
        else//if longitude
            if (coord >= 0) //determines how many minutse east or west, 0 is Grinwich  larger then 0 is east smaller is west
            direction = 'N';
        else
        {
            direction = 'S';
            coord *= -1;
        }
        //determines the various sexagesimal factors
        int deg = ((int)coord / 1);
        int min = (((int)coord % 1) * 60) / 1;
        double sec = (((coord % 1) * 60) % 1) * 60;
        const string quote = "\"";
        string toReturn = deg + "° " + min + $"' " + sec + quote + direction;
        return toReturn;
    }

    // computes half a versine of the angle
    public static double Hav(double radian)
    {
        return Math.Sin(radian / 2) * Math.Sin(radian / 2);
    }

    //returns an angle in radians
    public static double Radians(double degree)
    {
        return degree * Math.PI / 180;
    }

    //receiving 2 points the haversine formula returns the distance (in km) between the 2
    public static double Haversine(double lon1, double lat1, double lon2, double lat2)
    {
        const int RADIUS = 6371;//earths radius in KM

        double radLon = Radians(lon2 - lon1);//converts differance btween the points to radians
        double radLat = Radians(lat2 - lat1);
        double havd = Hav(radLat) + (Math.Cos(Radians(lat2)) * Math.Cos(Radians(lat1)) * Hav(radLon));//haversine formula determines the spherical distance between the two points using given versine
        double distance = 2 * RADIUS * Math.Asin(havd);
        return distance;
    }
    //function determines the distance between a point and station/customer
    //public static double Distance(double lonP1, double latP1, double lonP1, double latP)
    //{
    //    if (ID > 9999)//if its a customer
    //        foreach (Customer cus in DalObject.DataSource.customerList) { if (int.Parse(cus.ID) == ID) return Haversine(lonP, latP, cus.longitude, cus.latitude); }

    //    // DataSource.customerList.ForEach(c => { if (int.Parse(c.ID) == ID) { return Haversine(lonP, latP, c.longitude, c.latitude); });//returns in a string the distnace between the  customer and given point                   
    //    else//its a station
    //        //DataSource.stationsList.ForEach(s => { if (s.ID == ID) { return Haversine(lonP, latP, s.longitude, s.latitude); });//returns in a string the distnace between the  station and given point                   
    //        foreach (Station KingsX in DalObject.DataSource.stationList) { if (KingsX.ID == ID) return Haversine(lonP, latP, KingsX.Longitude, KingsX.Latitude); }
    //    return 0.0;// default return
    //}
}
