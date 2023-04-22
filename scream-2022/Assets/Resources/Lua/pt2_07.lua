wait(1)

enterNVL()
enter('SUMI', 'd')
enter('BRAULIO', 'b')
speak('BRAULIO', "Ariel? Are you alright? You're breathing pretty hard.")
setWake(-1)
speak('ARIEL', "I'm fine. I'm not sure about this dream though.")
speak('SUMI', "Did you see Grandmother? Or me?")
speak('ARIEL', "Only you. The ending might be abrupt. I'm sorry. This probably isn't a good dream to work with.")
speak('SUMI', "That's fine.")
speak('ARIEL', "Maybe I should retry.")
expr('SUMI', 'intense')
speak('SUMI', "You've already failed once. How many tries does this usually take?")
expr('SUMI', nil)
speak('SUMI', "Just play back the dream, please.")
speak('BRAULIO', "You're the boss!")
exitNVL()

wait(.6)

fade('black')
setSwitch('pt2_07', true)
teleport('Gazer', 'pt2_target', 'NORTH', true)
fade('normal')

wait(.6)
rotateTo('pt2_look')
wait(.6)

enterNVL()
enter('BRAULIO', 'b', 'determined')
enter('SUMI', 'c')
speak('BRAULIO', "Here, put this over your eyes, and then take this tablet.")
speak('SUMI', "I'm excited. Let's begin.")
enter('BRAULIO', 'b', nil)
speak('BRAULIO', "Sweet dreams!")
exitNVL()

wait(1)
bootGazer(true)
wait(1.7)

enterNVL()
enter('BRAULIO', 'c')
speak('ARIEL', "Is she under yet?")
speak('BRAULIO', "Looks like it. She'll enter REM sleep soon and then I'll turn this on.")
speak('ARIEL', "It's really not a pleasant dream.")
expr('BRAULIO', 'angry')
speak('BRAULIO', "I'm pretty sure any dream with her in it would become a nightmare.")
expr('BRAULIO', nil)
speak('ARIEL', "What was that earlier about oneirophrenia? What is it, and why wasn't I told about it?")
speak('BRAULIO', "I'm really sorry about that. Really! But it shouldn't affect you because you're just the dreamer, not the receiver, right?")
speak('ARIEL', "You didn't answer what it is. And you might not be able to tell, but I'm upset about this.")
speak('BRAULIO', "It...")
expr('BRAULIO', 'determined')
speak('BRAULIO', "Sumi said it before. Too much Bluepill and you'll mix up dreams and reality. Too long asleep, you might experience, um...")
expr('BRAULIO', 'angry')
speak('BRAULIO', "I think you said it before. Ego death.")
expr('BRAULIO', nil)
speak('BRAULIO', "Loss of personhood. No identity.")
speak('ARIEL', "Is this why Chris left?")
speak('BRAULIO', "He hasn't left for good. And no, I don't think this is why he's taking a break, at least not directly.")
speak('ARIEL', "Then why isn't he here?")
speak('BRAULIO', "I think he did what he did because he cares very deeply about you and Noemi. That's all I - ")
exit('BRAULIO')
enter('SUMI', 'c', 'surprise')
speak('SUMI', "Eyaaah! Ahh! Ampio aho! Ampio!")
speak('ARIEL', "Sumi! Braulio, unhook her!")
exitNVL()

wait(.7)
bootGazer(false)
wait(.7)

enterNVL()
enter('BRAULIO', 'a')
enter('SUMI', 'c')
speak('ARIEL', "Sumi, wake up. You're fine. You're right here.")
expr('SUMI', nil)
speak('SUMI', "Iza iano? Who?")
speak('BRAULIO', "Is she okay?")
speak('SUMI', "How did...")
speak('SUMI', "Ha... Ariel and Braulio. It's just you two.")
speak('ARIEL', "I knew that dream wasn't lucid. I hope I'm not losing my touch.")
expr('SUMI', 'surprise')
speak('SUMI', "I saw... her. She...")
expr('SUMI', 'intense')
speak('SUMI', "Go away. Let me catch my breath.")
speak('BRAULIO', "I'm really sorry about this Ms. Chey!")
speak('SUMI', "Who is Sara?")
speak('ARIEL', "I don't know any Saras, sorry.")
expr('BRAULIO', 'unsure')
speak('BRAULIO', "Can I get you something to drink?")
expr('SUMI', nil)
speak('SUMI', "Just... leave me alone.")
speak('ARIEL', "I'm going to talk to Noemi. Look after Sumi, okay?")
expr('BRAULIO', 'determined')
speak('BRAULIO', "Got it.")
exitNVL()

setSwitch('clear_sprites', true)
teleport('F2', 'gazer', 'NORTH')