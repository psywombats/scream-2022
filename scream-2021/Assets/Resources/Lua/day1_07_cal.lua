speak("Dr. Cooper", "Hey! Tess!", 'd1_cal1')
speak("Tess", "Dr. Cooper.")
speak("Dr. Cooper", "Did you lose track of time or what? I couldn't find you in your room, but you're late for your 4 o'clock!", 'd1_cal1')
faceToward('lia_bot')
speak("Tess", "I have to go, Lia. I'll see you back in the room?")
speak("Lia", "Alright. Bye Tess, Dr. Cooper.", 'lia_bot')
speak("Dr. Cooper", "It's Cal! I see Tess has been filling your head with weird ideas already. Sheesh.", 'd1_cal1')
faceToward('d1_cal1')
speak("Tess", "Let's just go.")
speak("Dr. Cooper", "Your wish is my command.", 'd1_cal1')
untrackCamera()
walk('hero', 7, 'EAST', false)
walk('d1_cal1', 7, 'EAST')

setSwitch('exam_cal_appears', true)
teleport('LabA', 'chair_target', 'SOUTH')
wait(1.5)

speak("Dr. Cooper", "Alright. Are you comfortable?", 'bottom')
speak("Tess", "I'm fine.")
speak("Dr. Cooper", "You know the drill by now. I'm going to ask a series of questions to assess your cognitive state and powers of specific recall.", 'bottom')
speak("Dr. Cooper", "Even if you feel like you don't know the answer, think carefully before you respond.", 'bottom')
speak("Dr. Cooper", "Coming up with the right answer is less important than giving us a sample of your brainwaves to analyze.", 'bottom')
speak("Dr. Cooper", "Ready?", 'bottom')
speak("Tess", "As always.")
speak("Dr. Cooper", "Then here's the first question...", 'bottom')
wait(.7)
face('cal', 'EAST')
wait(.9)
face('cal', 'NORTH')
wait(.7)
speak("Dr. Cooper", "Could you please tell me your full name?", 'bottom')
setDepthMult(.9)

choice("...Tess.", "I don't know.")
if choice_result == 0 then
    speak("Dr. Cooper", "Your full name.", 'bottom')
else
    speak("Dr. Cooper", "Even just part of your name.", 'bottom')
    speak("Tess", "I can't remember.")
end

speak("Tess", "I am patient #2876. Two Eight Seven Six. T-E-S-S.")
walk('cal', 1, 'WEST')
wait(.1)
face('cal', 'NORTH')
wait(.1)
walk('cal', 1, 'EAST')
wait(.1)
face('cal', 'NORTH')
speak("Dr. Cooper", "You'll remember one of these days Tess. I know it.", 'bottom')
wait(1.0)
speak("Dr. Cooper", "Next question... Could you please tell me the current year?", 'bottom')
setDepthMult(.8)

choice("2021?", "I don't know.")
if choice_result == 0 then
    speak("Dr. Cooper", "I'm afraid it hasn't been 2021 for quite some time.", 'bottom')
    speak("Tess", "Then tell me what year it is.")
else
    speak("Dr. Cooper", "Think harder.", 'bottom')
    speak("Tess", "I don't know. I don't remember. You never tell me.")
end

wait(.3)
face('cal', 'WEST')
wait(1)
face('cal', 'NORTH')
wait(.6)
speak("Dr. Cooper", "Moving on...")
speak("Dr. Cooper", "Can you tell me your place of birth?")
setDepthMult(.75)

choice("Allsaints' Hospital.", "I don't know.")
if choice_result == 0 then
    speak("Dr. Cooper", "No, that's the hospital you're in now. We don't even have a maternity ward.", 'bottom')
else
    speak("Dr. Cooper", "That's alright. Plenty of kids don't know that.", 'bottom')
end

wait(.5)
speak("Dr. Cooper", "Okay! Drumroll please, this is the last question. Final Jeopardy!", 'bottom')
walk('cal', 1, 'NORTH')
speak("Dr. Cooper", "Could you tell me please, what is my purpose in life?", 'bottom')
setDepthMult(.65)

choice("To tease me with stupid questions, apparently.", "I don't know.")
if choice_result == 0 then
    speak("Dr. Cooper", "Sheesh, I can't catch a break today.", 'bottom')
else
    speak("Dr. Cooper", "Sheesh, you'd think you were born with that scowl on your face, Tess. Lighten up a bit.", 'bottom')
end

setDepthMult(1)
walk('cal', 2, 'EAST')
face('cal', 'WEST')
wait(0.6)
speak("Dr. Cooper", "Thanks for bearing with me.", 'cal')
speak("Dr. Cooper", "We'll move on and take your vitals and do a basic checkup from here, although...", 'cal')
speak("Dr. Cooper", "Next time please think a little harder about the questions, okay?", 'cal')
speak("Dr. Cooper", "It's really important for my research. I'm really really close!", 'cal')
speak("Tess", "Whatever you say.")

setSwitch('day1_07_cal', true)
setSwitch('spawn_lia', false)
teleport('CommonRoom', 'in')
setSwitch('exam_cal_appears', false)
