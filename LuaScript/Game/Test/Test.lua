
---F1 到 F12 测试

function Test_F1()
	---@type UnityEngine.U2D.SpriteAtlas
	local obj = ResManager:Load("Atlas/UI/UIItem")
	log(obj:GetSprite("item_1"),"yellow")
end

function Test_F2()
	local test = require("Game.Test.ProtoTest")
	test:Start()
end

function Test_F3()
	local test = require("Game.Test.CoroutineTest")
	test:Start2()
end

function Test_F4()
	local test = require("Game.Test.OtherTest")
	test:TableTest()
end

function Test_F5()
	local test = require("Game.Test.HttpTest")
	test:Start()
end

function Test_F6()
	local test = require("Game.Test.OtherTest")
	test:File()
end

function Test_F7()
	logAssert(1>2,"true4324234")
end

function Test_F8()

end

function Test_F9()

end

function Test_F10()
	SceneApplication.ChangeState(MetaScene)
end

function Test_F11()

end

function Test_F12()
	SingleNet.Login().SendDeleteAccount()
end

----------------F1-F2 下排数字键--------------------
function Test_Alpha0() end
function Test_Alpha1() end
function Test_Alpha2() end
function Test_Alpha3() end
function Test_Alpha4() end
function Test_Alpha5() end
function Test_Alpha6() end
function Test_Alpha7() end
function Test_Alpha8() end
function Test_Alpha9() end
