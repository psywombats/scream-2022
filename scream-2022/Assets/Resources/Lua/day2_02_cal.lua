speak("Dr. Cooper", "Oh! Tess. It's just you. Phew.", 'd2_cal0')
speak("Tess", "Don't I have a checkup this morning?")
speak("Dr. Cooper", "No, no, you definitely do. You just caught me while I was thinking is all.", 'd2_cal0')
speak("Tess", "What exactly was it that you were doing last night?")
speak("Dr. Cooper", "Last night? Oh, haha, oh that, haha. Just a joke. You know me.", 'd2_cal0')
speak("Tess", "You should test yourself for N9.")
speak("Tess", "You were exposed to my eyes for a very long time.")
speak("Dr. Cooper", "I don't really know what you're getting at, Tess.", 'd2_cal0')
speak("Dr. Cooper", "I didn't get a whole lot of sleep last night, that's true...", 'd2_cal0')
speak("Dr. Cooper", "...but I'm pretty dumb, so if this is a practical joke, you'll have to explain it.", 'd2_cal0')
speak("Tess", "What, did you forget?")
wait(.5)
speak("Dr. Cooper", "Tess, when you're an adult, you'll learn that there are certain things that are better left unremembered.", 'd2_cal0')
speak("Dr. Cooper", "Let's just get on with the checkup, shall we?", 'd2_cal0')
wait(.9)
speak("Tess", "Yes Dr. Cooper.")

fadeOutBGM(1)
fade('fade_long')
setSwitch('exam_cal_appears', true)
teleport('LabA', 'chair_target', 'SOUTH')
playBGM('extraroom')
wait(1.5)

speak("Dr. Cooper", "Now then...", 'cal')
speak("Dr. Cooper", "I'm going to ask a series of questions to assess your cognitive state and powers of specific recall.", 'cal')
speak("Dr. Cooper", "Even if you feel like you don't know the answer, think carefully before you respond. Are you ready?", 'cal')
speak("Tess", "As ever.")
speak("Dr. Cooper", "Then could you please tell me your full name?", 'cal')
setDepthMult(.9)

choice("Tess.", "I don't know.")
if choice_result == 0 then
    setDepthMult(.8)
    speak("Dr. Cooper", "Your true name.", 'cal')
    speak("Tess", "That's all I remember.")
    speak("Dr. Cooper", "Think harder.", 'cal')
    speak("Tess", "Two-eight-seven-six. Tess.")
    speak("Dr. Cooper", "No, that's not a name. That's a stupid moniker that Dr. Gray gave you.", 'cal')
else
    speak("Dr. Cooper", "You're not even trying.", 'cal')
    speak("Tess", "You already know that I don't remember.")
    speak("Dr. Cooper", "You have to try. I neeeed you to try, Tess.", 'cal')
end

wait(.7)
setDepthMult(.75)
speak("Dr. Cooper", "Now, what is your real, true name? Beyond Tess.", 'cal')
speak("Tess", "I remember no other name but Tess.")
face('cal', 'EAST')
wait(.4)
speak("Dr. Cooper", "...Fine.", 'cal')
face('cal', 'NORTH')
wait(.2)
setDepthMult(.7)
speak("Dr. Cooper", "Could you please tell me the current year?", 'cal')

choice("2022?", "I don't know.")
if choice_result == 0 then
    speak("Dr. Cooper", "Wasn't that your answer yesterday?", 'cal')
    speak("Tess", "I don't remember.")
else
    speak("Dr. Cooper", "I guess that makes sense...", 'cal')
end

speak("Tess", "If I ever get it right, would you tell me?")
walk('cal', 1, 'NORTH')
wait(.3)
speak("Dr. Cooper", "Tess, do you understand how close I am here?", 'cal')
speak("Dr. Cooper", "This is the breakthrough. This will change my life. Yours too.", 'cal')
face('cal', 'SOUTH')
speak("Dr. Cooper", "I just need need a few more samples, so at least try to cooperate. I know you can do it.", 'cal')
speak("Tess", "Next question then.")
face('cal', 'NORTH')
wait(.3)
speak("Dr. Cooper", "Could you please tell me your date of birth?", 'cal')

choice("March 4th?", "I don't know.")
if choice_result == 0 then
    speak("Dr. Cooper", "That's... interesting.", 'cal')
    speak("Tess", "Am I right? I don't know where that date came from.")
    speak("Dr. Cooper", "Whether you're right or wrong doesn't really matter.", 'cal')
else
    speak("Dr. Cooper", "I understand you're mad at me, Tess.", 'cal')
    speak("Tess", "I'm just confused.")
    speak("Dr. Cooper", "Really. I think you're toying with me.", 'cal')
    speak("Tess", "I assure you I am not.")
end

speak("Dr. Cooper", "Okay, okay, enough of that. Let's go on to the final question. Are you ready?", 'cal')
speak("Tess", "I'm always ready.")
face('cal', 'SOUTH')
setDepthMult(.55)
speak("Dr. Cooper", "What...", 'cal')
wait(.7)
face('cal', 'NORTH')
setDepthMult(1)
speak("Dr. Cooper", "What do you call a tuna with a top hat?", 'cal')

choice("Fishy?", "I don't know.")
if choice_result == 0 then
    speak("Dr. Cooper", "Nope!", 'cal')
else
    speak("Dr. Cooper", "You're no fun Tess.", 'cal')
end

speak("Dr. Cooper", "Sofishticated! Get it?", 'cal')
speak("Tess", "I hope you weren't analyzing my brain waves for that one. They probably zeroed out.")
speak("Dr. Cooper", "Haha, got you. Let's get the rest of this over with and then you can get back to your day.", 'cal')

setSwitch('day2_02_cal', true)
fadeOutBGM(.5)
teleport('HallA', 'd2_target')
playBGM('day')
