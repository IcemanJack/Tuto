var eventsData;
$(document).ready(function () {
    $('#calendar').weekCalendar(
        {
            // the calendar's title
            title: function(data, calendar) {
                return "";
            },

            // the event's title
            eventHeader: function(calEvent, calendar) {
                var timeBlock = getTimeBlockFromCalEvent(calEvent);
                return timeBlock.start + " à " + timeBlock.end;
            },

            // the days' headers
            getHeaderDate: function(date, calendar) {
                return getDayNameFromDate(date);
            },
      
            // the event's body
            eventBody: function(calEvent, calendar) {
                return "";
            },

            data: function(start, end, callback) {
                if (eventsData === undefined) {
                    callback([]);
                } else {
                    callback(eventsData);
                }
            },

            timeslotsPerHour: 1,

            timeslotHeight: 60,

            daysToShow: 5,

            businessHours: { start: 8, end: 18, limitDisplay: true },

            buttons: false,

            hourLine: false,

            height: function($calendar) {
                return ($(window).height() /1.30) - $('h1').outerHeight(true);
            },

            eventClick: function(calEvent, $event) {
                $event.remove();
            }
        });
});

// loads json data in calendar
function loadCalendarData(jsonData) {
    
    var initialDateFormat = "DD-MM-YYYY hh:mm a";

    var eventData = new Array();

    var initialData = JSON.parse(jsonData);
    for (var i = 0; i < initialData.length; i++) {
        var currentInitialEvent = initialData[i];
        var initialStartDate = "0" + currentInitialEvent.day.toString() + "-07-2013 " + currentInitialEvent.start;
        var initialEndDate = "0" + currentInitialEvent.day.toString() + "-07-2013 " + currentInitialEvent.end;

        var currentNewEvent = {};
        currentNewEvent.id = i + 1;
        currentNewEvent.start = moment(initialStartDate, initialDateFormat).toDate();
        currentNewEvent.end = moment(initialEndDate, initialDateFormat).toDate();
        currentNewEvent.title = "test" + i;

        eventData.push(currentNewEvent);
    }

    eventsData =
    {
        events : eventData
    };

    $("#calendar").weekCalendar("refresh");
}

 // returns an array of timeblocks from the calendar
function getCalendarData()
{
    var events = $("#calendar").weekCalendar("serializeEvents");
    var parsedEvents = new Array();

    for (var i = 0; i < events.length; i++) 
    {
        parsedEvents.push(getTimeBlockFromCalEvent(events[i]));
    };

    return JSON.stringify(parsedEvents);
} 

/**
 * Returns a timeBlock object from a calendar event.
 * @param  {date} the date to convert
 * @return {timeBlock} an object containing a start time, an end time and a day
 */
function getTimeBlockFromCalEvent(calEvent)
{
    var timeBlock = {};

    var format = "hh:mm a";
    timeBlock.start = moment(calEvent.start.toString()).format(format);
    timeBlock.end = moment(calEvent.end.toString()).format(format);
    timeBlock.day = calEvent.start.getDay();

    return timeBlock;
}

var days = new Array();
days.push("Lundi");
days.push("Mardi");
days.push("Mercredi");
days.push("Jeudi");
days.push("Vendredi");

function getDayNameFromDate(date)
{
    // getDay() returns the day index starting from 1
    return days[date.getDay() - 1];
}

function getDayNumberFromName(name)
{
    for (var i = 0; i < days.length; i++)
    {
        if (days[i] == name) {
            return i + 1;
        }
    }
    return 1;
}