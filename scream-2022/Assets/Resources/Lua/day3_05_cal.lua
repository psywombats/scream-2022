speak("Dr. Cooper", "Ahh if it isn't Tess. There you are!", 'd3_cal0')
speak("Lia", "Hi Cal. You already know Tess?", 'lia_bot')
speak("Dr. Cooper", "Yep, I know she's late for our 4 o'clock. Haha! This way, Tess. And catch you later, ummm...", 'd3_cal0')
speak("Lia", "I'm Lia.", 'lia_bot')
speak("Dr. Cooper", "Right, Lia. See you later Lia! Haha!", 'd3_cal0')

fadeOutBGM(1.5)
untrackCamera()
walk('hero', 7, 'EAST', false)
walk('d3_cal0', 7, 'EAST')

setSwitch('exam_cal_appears', true)
teleport('LabA', 'chair_target', 'SOUTH')
playBGM('extraroom')
wait(1.5)

speak("Dr. Cooper", "Tess. Tess Tess Tess Tess Tess.", 'cal')
walk('cal', 1, 'WEST')
wait(.1)
face('cal', 'NORTH')
wait(.1)
walk('cal', 1, 'EAST')
wait(.1)
face('cal', 'NORTH')
speak("Dr. Cooper", "I'm going to ask a series of questions to assess your cognitive state and powers of specific recall.", 'cal')
speak("Tess", "Dr. Cooper, how long have I been in Ward #6?")
speak("Dr. Cooper", "Tess Tess Tess. What a question.", 'cal')
speak("Tess", "Longer than a day?")
speak("Dr. Cooper", "You crack me up Tess. Tess Tess Tess. You've been here almost longer than me. I think.", 'cal')
speak("Tess", "Everyone in the ward forgot me.")
speak("Dr. Cooper", "Well it is an N9 ward after all. Haha! Now, are you comfy? Are you ready?", 'cal')
speak("Tess", "This doesn't seem important right now.")
speak("Dr. Cooper", "Fair enough. I've already broken through, but humor me, for old time's sake.", 'cal')
face('cal', 'EAST')
wait(.5)
speak("Dr. Cooper", "Now then...")
wait(.8)
face('cal', 'NORTH')
setDepthMult(.75)
speak("Dr. Cooper", "What was the first question?", 'cal')

choice("What is my name?", "I don't know.")
if choice_result == 0 then
    speak("Dr. Cooper", "Tess, I think.", 'cal')
else
    speak("Dr. Cooper", "That makes two of us!", 'cal')
end

speak("Dr. Cooper", "Next question... The second question.", 'cal')
speak("Dr. Cooper", "Could you please tell me the second question?", 'cal')

choice("What is the current year?", "I don't know.")
if choice_result == 0 then
    speak("Dr. Cooper", "That's one of many petty details that I've exchanged for truer knowledge of the nature of reality.", 'cal')
    speak("Dr. Cooper", "Or something like that!", 'cal')
else
    speak("Dr. Cooper", "Spooky. I don't know it either.", 'cal')
end

speak("Dr. Cooper", "Ahaha. Just pulling your leg. Of course I know the year.", 'cal')
speak("Dr. Cooper", "Now, the moment of truth. The razor's edge! The final question. The final answer.", 'cal')
setDepthMult(.5)
walk('cal', 1, 'NORTH')
wait(1)
speak("Dr. Cooper", "TELL ME, OH TELL ME, WHAT IS THE PURPOSE OF MY LIFE?", 'cal')
wait(.7)

choice("There is none.", "I don't know.")
if choice_result == 0 then
    setDepthMult(.7)
    speak("Dr. Cooper", "Really? Is that really the answer?", 'cal')
    speak("Tess", "I do not know or care.")
else
    setDepthMult(.4)
    speak("Dr. Cooper", "No. No! No no no!", 'cal')
end

setDepthMult(.55)
speak("Dr. Cooper", "Tess. 2876. Cecily. Whatever your name is. I need you!", 'cal')
walk('cal', 1, 'NORTH')
speak("Dr. Cooper", "Tell me what it is I was meant to do!", 'cal')
speak("Tess", "Who do you think I am?")
speak("Dr. Cooper", "You've done it! You're the culmination of everything we've been working towards!", 'cal')
speak("Dr. Cooper", "Tell me the future. Tell me it wasn't all for nothing, all of that, that...", 'cal')
wait(.8)
walk('cal', 1, 'SOUTH')
face('cal', 'NORTH')
setDepthMult(1)
speak("Dr. Cooper", "Ahem.", 'cal')
walk('cal', 1, 'WEST')
face('cal', 'EAST')
speak("Dr. Cooper", "Thanks for bearing with me. We'll move on and take your vitals and do a basic checkup from here.", 'cal')
speak("Dr. Cooper", "Next time please think a little harder about the questions, okay?", 'cal')
speak("Dr. Cooper", "It's really important for my research.", 'cal')

fade('black')
fadeOutBGM(1)
wait(1)
setSwitch('spawn_lia', false)
setSwitch('exam_cal_appears', false)
intertitle("THOUGH STARS NUMBER IN THE TRILLIONS\nAND GRAINS OF DUST STRETCH INFINITE\nTHERE IS ONLY ONE TRUTH\n\nTHAT OF THE\nI AM\n\n\nNIGHT_3")
setSwitch('night', true)
setSwitch('d3_clear', true)
teleport('RoomYours', 'start')