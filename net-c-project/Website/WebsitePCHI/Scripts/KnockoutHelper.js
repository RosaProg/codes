

// Progress bar
ko.bindingHandlers.updateProgressBar = {
    update: function (element, valueAccessor, allBindings) {
        var currentValue = ko.utils.unwrapObservable(valueAccessor());
        //var currentValue = allBindings.get('CurrentValue');
        var maxValue = ko.utils.unwrapObservable(allBindings.get('MaximumValue'));


        // Get the percentage to change the style
        var percentage = (currentValue * 100) / maxValue;
        if (percentage) {
            intPercentageValue = Math.round(percentage);
            $('#progress-bar-percentage').width(intPercentageValue + '%');
            $('#progress-bar-text').text(currentValue + ' of ' + maxValue);
        }
    }
};

