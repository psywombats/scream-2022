enterNVL()
enter('CHRIS', 'c')
if not getSwitch('pt1_13') then
	speak('CHRIS', "Ariel. I was beginning to think you wouldn't show. Listen, I'm sorry, forget what I said about _____. This is more serious.")
	speak('CHRIS', "It's Noemi's rats. She had them undergo _____ to induce _____, but I think it's fatal.")
	speak('CHRIS', "Maybe not immediately, but once _____ for too long, it's unavoidable.")
	speak('CHRIS', "Did that happen to you and Noemi?")
	speak('CHRIS', "Ariel?")
	speak('CHRIS', "I don't understand _____. If _____ happened to ______ then what are we _____?")
	speak('CHRIS', "Mirroring causes _____ like a timer. Inevitable. You've proven that _____ . _____.")
	speak('CHRIS', "Ariel? Where are you going?")
	speak('ARIEL', "I'm dreaming.")
	speak('CHRIS', "You're not dreaming.")
	speak('ARIEL', "I have always been dreaming.")
	speak('CHRIS', "Ariel, listen to me!")
	speak('ARIEL', "I'll listen once I'm awake.")
else
	speak('CHRIS', "____!!")
end
exitNVL()

setSwitch('pt1_13', true)
setSwitch('nightmare_go', true)
