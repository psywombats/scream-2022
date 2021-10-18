speak("Dr. Cooper", "Oh! Tess. It's just you. Phew.")
speak("Tess", "Don't I have a checkup this morning?")
speak("Dr. Cooper", "No, no, you definitely do. You just caught me while I was thinking is all.")
speak("Tess", "What exactly was it that you were doing last night?")
speak("Dr. Cooper", "Last night? Oh, haha, oh that, haha. Just a joke. You know me.")
speak("Tess", "You should test yourself for N9.")
speak("Tess", "You were exposed for almost a whole minute.")
speak("Dr. Cooper", "I don't really know what you're getting at, Tess.")
speak("Dr. Cooper", "I didn't get a whole lot of sleep last night, that's true, but I'm pretty dumb so if this is a practical joke, you'll have to explain it.")
speak("Tess", "Did you forget?")
speak("Dr. Cooper", "Tess, when you're an adult, you'll learn that there are certain things that are better left unremembered.")
speak("Dr. Cooper", "Let's just get on with the checkup, shall we?")
speak("Tess", "Yes Dr. Cooper.")
speak("Dr. Cooper", "I'm going to ask a series of questions to assess your cognitive state and powers of specific recall.")
speak("Dr. Cooper", "Even if you feel like you don't know the answer, think carefully before you respond. Are you ready?")
speak("Tess", "As ever.")
speak("Dr. Cooper", "Now, could you please tell me your full name?")

choice("Tess.", "I don't know.")
if choice_result == 0 then
    speak("Dr. Cooper", "Your true name.")
    speak("Tess", "That's all I remember.")
    speak("Dr. Cooper", "Think harder.")
    speak("Tess", "Two eight seven six.")
    speak("Dr. Cooper", "No, that's not a name. That's a stupid moniker that Dr. Gray gave you.")
else
    speak("Dr. Cooper", "You're not even trying.")
    speak("Tess", "You already know that I don't remember.")
    speak("Dr. Cooper", "You have to try.")
end

speak("Dr. Cooper", "Now, what is your real, true name? Beyond Tess.")
speak("Tess", "I remember no other name but Tess.")
speak("Dr. Cooper", "...Fine.")
speak("Dr. Cooper", "Could you please tell me the current year?")

choice("2022?", "I don't know.")
if choice_result == 0 then
    speak("Dr. Cooper", "Wasn't that your answer yesterday?")
    speak("Tess", "I don't remember.")
else
    speak("Dr. Cooper", "I guess that makes sense...")
end

speak("Tess", "If I ever get it right, would you tell me?")
speak("Dr. Cooper", "Tess, you understand how close I am here?")
speak("Dr. Cooper", "This is the breakthrough. This will change my life. Yours too.")
speak("Dr. Cooper", "I just need need a few more samples, so at least try to cooperate. I know you can do it.")
speak("Tess", "Next question then.")
speak("Dr. Cooper", "Could you please tell me your birth date?")

choice("March 4th", "I don't know.")
if choice_result == 0 then
    speak("Dr. Cooper", "That's... interesting.")
    speak("Tess", "Am I right? I don't know where that date came from.")
    speak("Dr. Cooper", "Whether you're right or wrong doesn't really matter.")
else
    speak("Dr. Cooper", "I understand you're mad at me, Tess.")
    speak("Tess", "I'm just confused.")
    speak("Dr. Cooper", "Really. I think you're toying with me.")
    speak("Tess", "I assure you I am not.")
end

speak("Dr. Cooper", "Okay, okay, enough of that. Let's go on to the final question. Are you ready?")
speak("Tess", "I'm always ready.")
speak("Dr. Cooper", "What do you call a tuna with a top hat?")

choice("Fishy?", "I don't know.")
if choice_result == 0 then
    speak("Dr. Cooper", "Nope!")
else
    speak("Dr. Cooper", "Oh come on Tess.")
end

speak("Dr. Cooper", "Sofishticated! Get it?")
speak("Tess", "I hope you weren't analyzing my brain waves for that one. They probably zeroed out.")
speak("Dr. Cooper", "Haha, got you. Let's get the rest of this over with and then you can get back to your day.")
