$(document).ready(function () {
    // Quiz Script Start //
    var questions = [{
        question: "What mode of gameplay are you looking for?",
        choices: ["Singleplayer", "Co-op", "Multiplayer"]
    }, {
        question: "What genre of game interests you?",
        choices: ["Action", "Adventure", "RPG", "Strategy", "Sports"]
    }, {
        question: "Are games with violence ok?",
        choices: ["Yes", "No"]
    }, {
        question: "What is your preferred setting?",
        choices: ["Sci-fi", "Fantasy", "Medieval", "Historical", "Horror"]
    }, {
        question: "Would you like games with additonal hardware support/requirements?",
        choices: ["No Support", "Controller Support", "VR Headset"]
    }];

    var questionCounter = 0; //Tracks question number
    var selections = []; //Array containing user choices
    var quiz = $('#quiz'); //Quiz div object

    // Display initial question
    displayNext();

    // Click handler for the 'next' button
    $('#next').on('click', function (e) {
        e.preventDefault();

        // Suspend click listener during fade animation
        if (quiz.is(':animated')) {
            return false;
        }
        choose();

        // If no user selection, progress is stopped
        if (selections[questionCounter] == "") {
            selections.pop(); // last element removed
            alert('Please make a selection!');
        } else {
            questionCounter++;
            displayNext();
        }
    });

    // Click handler for the 'prev' button
    $('#prev').on('click', function (e) {
        e.preventDefault();

        if (quiz.is(':animated')) {
            return false;
        }
        choose();
        selections.pop(); // last element removed
        selections.pop();
        questionCounter--;
        displayNext();
    });

    // Click handler for the 'Start Over' button
    $('#start').on('click', function (e) {
        e.preventDefault();

        if (quiz.is(':animated')) {
            return false;
        }
        questionCounter = 0;
        selections = [];
        displayNext();
        $('#start').hide();
    });

    // Animates buttons on hover
    $('.button').on('mouseenter', function () {
        $(this).addClass('active');
    });
    $('.button').on('mouseleave', function () {
        $(this).removeClass('active');
    });

    // Creates and returns the div that contains the questions and 
    // the answer selections
    function createQuestionElement(index) {
        var qElement = $('<div>', {
            id: 'question'
        });

        var header = $('<h2>Question ' + (index + 1) + ':</h2>');
        qElement.append(header);

        var question = $('<p>').append(questions[index].question);
        qElement.append(question);

        var radioButtons = createRadios(index);
        qElement.append(radioButtons);

        return qElement;
    }

    // Creates a list of the answer choices as radio inputs
    function createRadios(index) {
        var radioList = $('<ul>');
        var item;
        var input = '';
        for (var answer in questions[index].choices) {
            item = $('<li>');
            input = '<label><input type="radio" name="answer" value=' + answer + ' />';
            input += questions[index].choices[answer] + '</label>';
            item.append(input);
            radioList.append(item);
        }
        return radioList;
    }

    // Reads the user selection and pushes the value to an array
    function choose() {
        var selectedAnswer = $('input[name="answer"]:checked').parent('label').text();
        selections.push(selectedAnswer);
    }

    // Displays next requested element
    function displayNext() {
        quiz.fadeOut(function () {
            $('#question').remove();

            if (questionCounter < questions.length) {
                var nextQuestion = createQuestionElement(questionCounter);
                quiz.append(nextQuestion).fadeIn();
                //if (!(isNaN(selections[questionCounter]))) {
                //$('input[value=' + selections[questionCounter] + ']').prop('checked', true);
                //}

                // Controls display of 'prev' button
                if (questionCounter === 1) {
                    $('#prev').show();
                } else if (questionCounter === 0) {

                    $('#prev').hide();
                    $('#next').show();
                }
            } else {
                postAnswers();
                $('#next').hide();
                $('#prev').hide();
                $('#start').show();
                $("#quizModal").modal();
            }
        });
    }

    function postAnswers() {
        $.ajax({
            url: "SubmitQuiz",
            type: "POST",
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(selections),
            success: function () {
                alert("Hey it works");
            },
            error: function () {
                alert("Doesn't work");
            }
        });
    }
    // Quiz Script End //
    $("#add_to_wishlist_area_fail").click(function () {
        var x = document.getElementById("fail");
        x.style.display = "block";
    });
});