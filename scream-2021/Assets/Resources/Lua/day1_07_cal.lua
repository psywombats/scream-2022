speak("Dr. Cooper", "Hey! Tess!")
speak("Tess", "Dr. Cooper.")
speak("Dr. Cooper", "Did you lose track of time or what? I couldn't find you in your room, but you're late for your 4 o'clock!")
speak("Tess", "I have to go, Lia. I'll see you back in the room?")
speak("Lia", "Alright. Bye Tess, Dr. Cooper.")
speak("Dr. Cooper", "It's Cal! I see Tess has been filling your head with weird ideas already. Sheesh.")
speak("Tess", "Let's just go.")
speak("Dr. Cooper", "Your wish is my command.")

speak("Dr. Cooper", "Alright. Are you comfortable?")
speak("Tess", "I'm fine.")
speak("Dr. Cooper", "You know the drill by now. I'm going to ask a series of questions to assess your cognitive state and powers of specific recall.")
speak("Dr. Cooper", "Even if you feel like you don't know the answer, think carefully before you respond.")
speak("Dr. Cooper", "Coming up with the right answer is less important than giving us a sample of your brainwaves to analyze. Ready?")
speak("Tess", "As always.")
speak("Dr. Cooper", "Could you please tell me your full name?")

choice("...Tess.", "I don't know.")
if choice_result == 0 then
    speak("Dr. Cooper", "Your full name.")
else
    speak("Dr. Cooper", "Even just part of your name.")
    speak("Tess", "I can't remember.")
end

speak("Tess", "I am patient #2876. Two Eight Seven Six. T-E-S-S.")
speak("Dr. Cooper", "...")
speak("Dr. Cooper", "You'll remember one of these days Tess. I know it.")
speak("Dr. Cooper", "Next question... Could you please tell me the current year?")

choice("2021?", "I don't know.")
if choice_result == 0 then
    speak("Dr. Cooper", "I'm afraid it hasn't been 2021 for quite some time.")
    speak("Tess", "Then tell me what year it is.")
else
    speak("Dr. Cooper", "Think harder.")
    speak("Tess", "I don't know. I don't remember. You never tell me.")
end

speak("Dr. Cooper", "Moving on...")
speak("Dr. Cooper", "Can you tell me your place of birth?")

choice("Allsaints' Hospital", "I don't know.")
if choice_result == 0 then
    speak("Dr. Cooper", "No, that's the hospital you're in now. We don't even have a maternity ward.")
else
    speak("Dr. Cooper", "That's alright. Plenty of kids don't know that.")
end

speak("Dr. Cooper", "Okay! Drumroll please, this is the last question. Final Jeopardy!")
speak("Dr. Cooper", "Could you tell me please, what is my purpose in life?")
choice("To tease me with stupid questions, apparently.", "I don't know.")
if choice_result == 0 then
    speak("Dr. Cooper", "Sheesh, I can't catch a break today.")
else
    speak("Dr. Cooper", "Sheesh, you'd think you were born with that scowl on your face, Tess. Lighten up a bit.")
end

speak("Dr. Cooper", "Thanks for bearing with me. We'll move on and take your vitals and do a basic checkup from here, although...")
speak("Dr. Cooper", "Next time please think a little harder about the questions, okay? It's really important for my research.")
speak("Tess", "Whatever you say.")


