﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TSDFile
{
    /// <summary>
    /// TAS Heating Design Data
    /// </summary>
    public class HeatingDesignData
    {
        private TSD.HeatingDesignData pHeatingDesignData;

        internal HeatingDesignData(TSD.HeatingDesignData HeatingDesignData)
        {
            pHeatingDesignData = HeatingDesignData;
        }

        /// <summary>
        /// Gets TAS Heating Design Data Description
        /// </summary>
        /// <param name="HeatingDesignData">TSD Heating Design Data</param>
        /// <returns name="Description">TAS Heating Design Data Description</returns>
        /// <search>
        /// TAS, TBDDocument, TBDDocument, HeatingDesignData, Get Heating Design Data Description, tas, tsddocument, HeatingDesignData, HeatingDesignData
        /// </search>
        public static string Description(HeatingDesignData HeatingDesignData)
        {
            return HeatingDesignData.pHeatingDesignData.description;
        }

        /// <summary>
        /// Gets TAS Heating Design Data Name
        /// </summary>
        /// <param name="HeatingDesignData">TSD Heating Design Data</param>
        /// <returns name="Name">TAS Heating Design Data Name</returns>
        /// <search>
        /// TAS, TBDDocument, TBDDocument, HeatingDesignData, Get Heating Design Data Name, tas, tsddocument, HeatingDesignData, HeatingDesignData
        /// </search>
        public static string Name(HeatingDesignData HeatingDesignData)
        {
            return HeatingDesignData.pHeatingDesignData.name;
        }

        /// <summary>
        /// Gets TAS Heating Design Data GUID
        /// </summary>
        /// <param name="HeatingDesignData">TSD Heating Design Data</param>
        /// <returns name="Name">TAS Heating Design Data GUID</returns>
        /// <search>
        /// TAS, TBDDocument, TBDDocument, HeatingDesignData, Get Heating Design Data GUID, tas, tsddocument, HeatingDesignData, HeatingDesignData
        /// </search>
        public static string GUID(HeatingDesignData HeatingDesignData)
        {
            return HeatingDesignData.pHeatingDesignData.GUID;
        }

        /// <summary>
        /// Gets TAS Heating Design Data First Day
        /// </summary>
        /// <param name="HeatingDesignData">TSD Heating Design Data</param>
        /// <returns name="FirstDay">TAS Heating Design Data First Day</returns>
        /// <search>
        /// TAS, TBDDocument, TBDDocument, HeatingDesignData, Get Heating Design Data First Day, tas, tsddocument, HeatingDesignData, HeatingDesignData, FirstDay, firstday
        /// </search>
        public static int FirstDay(HeatingDesignData HeatingDesignData)
        {
            return HeatingDesignData.pHeatingDesignData.firstDay;
        }

        /// <summary>
        /// Gets Zone Data
        /// </summary>
        /// <param name="HeatingDesignData">TSD Heating Design Data</param>
        /// <param name="Index">Index</param>
        /// <returns name="ZoneData">Zone Data</returns>
        /// <search>
        /// TAS, TBDDocument, TBDDocument, HeatingDesignData, Get Heating Design Data ZoneData, tas, tsddocument, HeatingDesignData, HeatingDesignData, ZoneData, zonedata, Zone Data, zone data
        /// </search>
        public static ZoneData GetZoneData(HeatingDesignData HeatingDesignData, int Index)
        {
            return new ZoneData(HeatingDesignData.pHeatingDesignData.GetZoneData(Index));
        }

        /// <summary>
        /// Gets Zones Data
        /// </summary>
        /// <param name="HeatingDesignData">TSD Heating Design Data</param>
        /// <returns name="ZonesData">Zones Data</returns>
        /// <search>
        /// TAS, TBDDocument, TBDDocument, HeatingDesignData, Get Heating Design Data ZoneData, tas, tsddocument, heatingdesigndata, ZonesData, zonesdata, Zones Data, zones data
        /// </search>
        public static List<ZoneData> GetZonesData(HeatingDesignData HeatingDesignData)
        {
            List<ZoneData> aResult = new List<ZoneData>();

            int aIndex = 1;
            TSD.ZoneData aZoneData = HeatingDesignData.pHeatingDesignData.GetZoneData(aIndex);
            while (aZoneData != null)
            {
                aResult.Add(new ZoneData(aZoneData));
                aIndex++;
                aZoneData = HeatingDesignData.pHeatingDesignData.GetZoneData(aIndex);
            }

            return aResult;
        }

        /// <summary>
        /// Gets Zone Data
        /// </summary>
        /// <param name="HeatingDesignData">TSD Heating Design Data</param>
        /// <returns name="ZoneData">Zone Data</returns>
        /// <search>
        /// TAS, TBDDocument, TBDDocument, HeatingDesignData, Get Heating Design Data ZoneData, tas, tsddocument, HeatingDesignData, HeatingDesignData, ZoneData, zonedata, Zone Data, zone data
        /// </search>
        public static List<ZoneData> ZoneData(HeatingDesignData HeatingDesignData)
        {
            List<ZoneData> aZoneDataList = new List<ZoneData>();

            int aIndex = 0;
            TSD.ZoneData aZone = HeatingDesignData.pHeatingDesignData.GetZoneData(aIndex);
            while (aZone != null)
            {
                aZoneDataList.Add(new ZoneData(aZone));
                aIndex++;
                aZone = HeatingDesignData.pHeatingDesignData.GetZoneData(aIndex);
            }
            return aZoneDataList;
        }
    }
}
