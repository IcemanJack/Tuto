namespace Tuto.DataLayer
{
    public static class DataLayerConfig
    {
        public const int DEFAULT_HOURS_PER_SESSION = 2;
        public const int SCHEDULE_MINIMUM_TIME = 8;
        public const int SCHEDULE_MAXIMUM_TIME = 18;
        public const string SCHEDULE_TIME_FORMAT = "hh:mm tt";
        public const string SCHEDULE_JSON_SCHEMA =
            "{ \"type\": \"array\", \"items\": [{ \"type\" : \"object\",\"properties\" : {\"start\" : {\"type\" : \"string\"},\"end\" : {\"type\" : \"string\"},\"day\" : {\"type\" : \"number\"}},\"additionalProperties\" : false}]}";
    }
}