fadeOutBGM(.5)
setSwitch('jump_cal_appears', true)
wait(.1)
rotateTo('n1_cal')

speak("Dr. Cooper", "Tess.", 'n1_cal')
wait(1)
playBGM('night')
speak("Dr. Cooper", "Sheesh, don't look so shocked. It's just me. Or were you expecting someone else?", 'n1_cal')
speak("Tess", "You caught me by surprise.")
speak("Tess", "I didn't mean to intrude, I just got lost going to the bathroom.")
speak("Dr. Cooper", "Nice try, Tess, but I know you better than that.", 'n1_cal')
speak("Tess", "What're you doing here this late anyway?")
speak("Dr. Cooper", "This is my lab. I'm allowed to be here past midnight if I want.", 'n1_cal')
speak("Dr. Cooper", "You aren't though. I can't have you running around here pulling power switches.", 'n1_cal')
speak("Dr. Cooper", "I'm extreeemely close, and that'd really set my progress back! Haha.", 'n1_cal')
speak("Tess", "I'll go back to my room then.")
speak("Dr. Cooper", "Think you'll get off the hook so quickly?", 'n1_cal')
speak("Dr. Cooper", "Follow me.", 'n1_cal')
wait(.8)
speak("Dr. Cooper", "Oh come on. You know me. It's me, Cal. Here, I'll explain on the way.", 'n1_cal')

teleport('OfficeCal', 'target')
wait(1)
speak("Dr. Cooper", "So -- ", 'n1_cal')
rotateTo('n1_cal')
wait(.3)
speak("Dr. Cooper", "Don't tell Dr. Gray, but I'm agonizingly frustratingly close to getting to the root of Neural-9.", 'n1_cal')
speak("Tess", "A cure?")
speak("Dr. Cooper", "That's exactly it!", 'n1_cal')
speak("Dr. Cooper", "Er, I mean, I'm not working on a cure, but a solution to the sort of thinking where we'd require a cure.", 'n1_cal')
speak("Dr. Cooper", "We've been approaching the problem all wrong. Or at least I have. Who knows what Dr. Gray's been up to.", 'n1_cal')
speak("Dr. Cooper", "But the point is, we've been taking an epidemiological approach, or sometimes a neurological one, but...", 'n1_cal')
speak("Dr. Cooper", "You can't cure Neural-9 because it's not a disease.", 'n1_cal')
speak("Dr. Cooper", "It's an innate quality of being. More like a property. ", 'n1_cal')
speak("Dr. Cooper", "Like being a natural artist, or a quick thinker, or a piano prodigy. You're born with it.", 'n1_cal')
speak("Dr. Cooper", "Make sense?", 'n1_cal')
speak("Tess", "None.")
wait(.5)
speak("Dr. Cooper", "Actually... This is impeccable timing.", 'n1_cal')
speak("Dr. Cooper", "Maybe it's just my fortune that you showed up here tonight Tess. Or did you know I'd be here in advance?", 'n1_cal')
speak("Tess", "I did not.")
speak("Dr. Cooper", "All the same, I'm glad it's you.", 'n1_cal')
speak("Dr. Cooper", "Now please hold still.", 'n1_cal')
wait(1)
speak("Tess", "Alright.")
speak("Dr. Cooper", "Keep your gaze steady.", 'n1_cal')
speak("Tess", "Alright?")
speak("Dr. Cooper", "Ready?", 'n1_cal')
wait(.5)
setSprite('n1_cal', 'cal_eyes')
wait(.7)
speak("Dr. Cooper", "Now... Look directly into my eyes.", 'n1_cal')
card('eyes_green')
speak("Dr. Cooper", "Don't blink. Don't waver. I want to truly see!", 'n1_cal')
wait(.8)

fade('black')
wait(1)
fadeOutBGM(1)
setSwitch('night2_04_cal', true)
intertitle("A BROKEN CLOCK IS BROKEN\nCLOCK IS RIGHT DAY IS BROKEN\nBROKEN DAY TWICE A CLOCK IS\nA CLOCK IS BROKEN TWICE\n\nRIGHT?\n\nDAY_2")
setSwitch('night', false)
setSwitch('n1_clear', true)
teleport('RoomYours', 'start')
