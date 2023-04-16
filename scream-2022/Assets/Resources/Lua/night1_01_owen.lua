rotateTo('n1_owen0')

if getSwitch('night1_01_owen') then
    speak("Owen", "The key should open the service stairwell from the common room.", 'n1_owen0')
    speak("Tess", "I won't let you down.")
    return
end

startle()
speak("Owen", "Oh. Tess. You're early.", 'n1_owen0')
speak("Tess", "No. It's midnight.")
speak("Owen", "Really? I guess so. But you could at least knock next time...", 'n1_owen0')
wait(.6)
speak("Tess", "What did you want to talk about? You had a lead on Cecily?")
speak("Owen", "You just jump right into it, don't you? Let's start with something normal first.", 'n1_owen0')
speak("Owen", "Did Cooper change anything up today?", 'n1_owen0')
speak("Tess", "Same questions as always.")
speak("Owen", "Why is he only interested in your brainwaves? It doesn't make any sense.", 'n1_owen0')
speak("Tess", "You always say it doesn't make sense.")
speak("Owen", "Because it never has. Sure, Neural-9 is a neurodegenerative disease...", 'n1_owen0')
speak("Owen", "But so are Parkinson's, Alzheimer's, and so on.", 'n1_owen0')
speak("Owen", "The dangerous part is the infection vector. Eye contact. That's the unique part.", 'n1_owen0')
speak("Owen", "Shouldn't they be studying that, and not what's going on in our brains? Who cares about that. Just stop the spread.", 'n1_owen0')
speak("Owen", "Who knows. Maybe they're all lying about the eye contact thing anyway.", 'n1_owen0')
speak("Tess", "What makes you say that?")
walk('n1_owen0', 1, 'WEST')
faceOther('n1_owen0', 'hero')
wait(.2)
speak("Owen", "It makes no phsyical sense. That's the obvious reason.", 'n1_owen0')
rotateTo('n1_owen0')
speak("Owen", "But partially because we have no ways to verify anything with the outside world.", 'n1_owen0')
speak("Owen", "That's a theory Cecily and I had -- what symptom did Joey, Nadine, you, me, and her all had in common?", 'n1_owen0')
speak("Tess", "Couldn't remember the outside.")
speak("Owen", "Yep. Everyone forgot something, but no one could remember a world outside Allsaints.", 'n1_owen0')
speak("Owen", "So Gray and the rest could lie to us, and as long as no one brings information from outside, we have no choice but to believe them.", 'n1_owen0')
speak("Owen", "N9 might not even be contagious. That could be a ruse to deny us any visitors. To deny us information.", 'n1_owen0')
speak("Tess", "What would the point of that be?")
speak("Tess", "Besides, Lia remembers the outside. We can just ask her.")
speak("Owen", "She does?", 'n1_owen0')
face('n1_owen0', 'SOUTH')
speak("Owen", "...", 'n1_owen0')
faceOther('n1_owen0', 'hero')
speak("Owen", "You don't find it strange that just when Cecily and I came up with this theory, she vanishes?", 'n1_owen0')
speak("Owen", "And some new girl shows up in her place, with information specifically to disprove it?", 'n1_owen0')
speak("Tess", "What I think is that you're paranoid.")
speak("Owen", "No, not paranoid, just cautious. Can you blame me, after Cecily?", 'n1_owen0')
speak("Tess", "I'd say you were nuts, but... Right. We really can't figure out what happened to her.")
speak("Owen", "We're old enough where if she died of N9, Gray would tell us. That lady delivers bad news like a postman delivers mail.", 'n1_owen0')
speak("Owen", "Patient confidentiality, maybe? But what Cecily left behind reads like a suicide note.", 'n1_owen0')
speak("Tess", "Not really.")
speak("Owen", "Okay, maybe not the note she left for you. But she was my best friend. She sounded like...", 'n1_owen0')
speak("Owen", "She knew something bad was going to happen and there was no stopping it.", 'n1_owen0')
speak("Owen", "I don't care about the rest of the doctors' lies, but I AM going to find Cecily.", 'n1_owen0')
speak("Tess", "I'm with you.")
speak("Owen", "Although... the lead I found isn't really a lead. It's this.", 'n1_owen0')
walk('n1_owen0', 1, 'WEST')
rotateTo('n1_owen0')
faceOther('n1_owen0', 'hero')
wait(.3)
--playSFX('get')
wait(.6)
speak("Tess", "A key?")
speak("Owen", "It opens the service stairwell from the common room.", 'n1_owen0')
speak("Tess", "What do you want me to do with it? Sneak out?")
speak("Owen", "If we ran away, the only thing waiting for us outside is a N9 outbreak.", 'n1_owen0')
speak("Owen", "But you could find another floor, a non-N9 floor, and and find some news of the outside world.", 'n1_owen0')
speak("Owen", "We need to do SOMETHING to confirm we're not just goldfishes in a bowl for these people.", 'n1_owen0')
speak("Tess", "Why me specifically?")
speak("Owen", "If you get caught, Cooper will pull strings and get you out of it. You're his favorite. You'll be fine.", 'n1_owen0')
speak("Owen", "And... it just sort of makes sense. I think Cecily left things in your hands.", 'n1_owen0')
speak("Tess", "I won't let you down.")
speak("Owen", "You shouldn't care about me -- I don't matter. Do it for her.", 'n1_owen0')
face('n1_owen0', 'SOUTH')

setSwitch('night1_01_owen', true)
