rotateTo('chris_11')

enterNVL()
enter('CHRIS', 'd')
speak('CHRIS', "Braulio! I'm glad I caught you before you left.")
enter('BRAULIO', 'b')
speak('BRAULIO', "You aren't usually here this late. What's up?")
speak('CHRIS', "I knew I'd heard the name Sumi Chey before. Look, here's an article on her.")
expr('BRAULIO', 'angry')
speak('BRAULIO', "Wait... That isn't her. That's an old woman.")
speak('CHRIS', "Sumi Chey is almost ninety years old. She's not an heiress or anything like that. She founded the whole damn congolomerate.")
expr('BRAULIO', 'unsure')
speak('BRAULIO', "Then that woman who was here today...")
speak('CHRIS', "They're not Sumi Chey. They're an imposter. Almost certainly a corporate spy from some overseas lab looking to steal Gazer.")
exitNVL()

--TODO: ding!
setSwitch('sumi_appears', true)
wait(0.1)
rotateTo('sumi_11')
wait(1)

enterNVL()
enter('SUMI', 'b', 'intense')
speak('SUMI', "Incorrect.")
enter('BRAULIO', 'd', 'unsure')
enter('CHRIS', 'e', 'pain')
speak('BRAULIO', "Sumi!")
speak('CHRIS', "I thought you'd left.")
speak('BRAULIO', "How much of that did you, um...")
speak('SUMI', "You should find a more private meeting space next time, Dr. Kowalski.")
expr('BRAULIO', nil)
speak('BRAULIO', "Uhh, heh heh, nice weather?")
expr('SUMI', nil)
speak('SUMI', "You're holding a picture of my grandmother, by the way. We share a name.")
expr('CHRIS', nil)
speak('CHRIS', "You expect us to believe that?")
speak('SUMI', "While I can't let you speak to her directly, feel free to contact anyone on the Chey Group board if you need them to officially confirm my identity.")
expr('SUMI', 'intense')
speak('SUMI', "Or ask me anything about the company. Anything Grandmother knew, I know. I am in every way her successor.")
expr('CHRIS', 'pain')
speak('CHRIS', "There certainly weren't any articles about a successor...")
expr('CHRIS', nil)
speak('SUMI', "We're very private. And Grandmother doesn't want her health in the news.")
expr('BRAULIO', 'determined')
speak('BRAULIO', "We're really sorry about the misunderstanding, Ms. Chey.")
expr('SUMI', nil)
speak('SUMI', "No need to worry.")
expr('SUMI', 'intense')
speak('SUMI', "Probably.")
expr('SUMI', nil)
speak('SUMI', "Ahaha, have a good night. I'll be back in the morning with a few of my people from legal. Bye now!")
wipe()
exit('CHRIS')
exit('BRAULIO')
exit('SUMI')
enter('SUMI', 'c', 'intense')
speak('SUMI', "And good night to you, Ariel.")
speak('ARIEL', "Ah! Good night.")
setSwitch('sumi_appears', false)
exitNVL()

wait(1)
rotateTo('chris_11')
wait(.3)
enterNVL()
enter('BRAULIO', 'b')
enter('CHRIS', 'd', 'pain')
speak('BRAULIO', "Well that was embarassing.")
expr('CHRIS', nil)
speak('CHRIS', "I'm still not sure about her.")
expr('BRAULIO', 'determined')
speak('BRAULIO', "You're going to check her story?")
speak('CHRIS', "Sure will. And I say it's 50/50 she comes back tomorrow.")
expr('BRAULIO', nil)
speak('BRAULIO', "I think we can trust her. This is just a misunderstanding, right?")
wipe()
exit('BRAULIO')
exit('CHRIS')
enter('BRAULIO', 'a')
enter('CHRIS', 'e')
speak('CHRIS', "Oh, Ariel. I didn't see you.")
speak('CHRIS', "Remember, the Floor 36 computer lab.")
speak('CHRIS', "I'll be downstairs. Good night, Braulio.")
exit('CHRIS')
speak('BRAULIO', "I'm taking off. Thank you so much for everything today Ariel!")
speak('BRAULIO',  "And I'm sure Sumi will back tomorrow with funding papers. Good night!")
setSwitch('pt1_11', true)
exitNVL()

