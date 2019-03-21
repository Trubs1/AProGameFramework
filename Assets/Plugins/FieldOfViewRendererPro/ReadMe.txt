// Description:游戏中单位的视野剔除显示
// Copyright (c) 2018 WangQiang(279980661@qq.com)
// Author:Trubs (WQ)
// Date:2018/08/08

游戏中单位的视野剔除显示

主要功能:
  1. 通过配档显示敌人视野的范围.
  2. 有障碍物的地方视野会剔除渲染.
  3. 自定义shader,通过美术给定的图片采样渲染出不同透明度有渐变效果的纹理.
  4. 代码简洁易懂,扩展性高.数据类型接口丰富,(如:可通过给定颜色改变视野颜色,以起到显示状态作用.)
  5. 性能开销可调整,高质量的情况也不会开销太大.
  6. 提供其他可行方案,供选择对比.


使用步骤: 导入下载的包体,打开里面的Demo场景即可预览使用.若存在问题或者需要修改请执行以下步骤.
	  1. 将在MainCamera上添加DrawHelper绘制器,并绑定材质FieldOfViewRenderer.
	  2. 在你的单位的眼睛或者虚拟点上添加Eye脚本(也可以在脚本里面添加一个eyeTrans的变量,这样脚本可以放在单位根节点,只需要初始化eyeTrans即可)
	  不明白可以看截图和视频

(之前在网上找到了一个通过摄像机剔除而渲染视野的方案,功能满足,但是开销太大,根本没法用到工程中(加入后20帧).于是自己直接通过OpenGL底层渲染视野.此方案不需要创建mesh,并且所有的视野图形都一批渲染出来,不会像mesh渲染那样可能分批次.之所以不叫FieldOfVisionRenderer是为了和网上的某方案区分)


更新:	2019/03/18 加入了框选功能 详情见FieldOfViewRenderer_andBoxSelect.unitypackage(Unity2018.3.5)

						Unity Version:2017.4.2f2  希望能帮到你,谢谢!


It is used for the enemy's vision field in the game.

Main functions:
1. Display the enemy's range of vision.
2. The vision field with obstacles won't be rendered.
3. Customize shader to render texture with gradient effect with different transparency by sampling images given by art staff.
4. The code is simple and easy to understand, with high expansibility. (for example, the view color can be changed by a given color to display the state).
5. It costs little even you set it to high quality. The performance cost can be adjusted.(It is better than some other's solution.)
6. Provide other suitable way for comparison and selection.


Use steps: Importing the package,openning the Demo scene,then you can preview it. If there are problems or need to modify, please perform the following steps.
1. Drag the DrawHelper(script) into to the MainCamera and Drag the FieldOfViewRenderer(material) into the Mat.
2. Add the Eye(script) to your unit's eye or virtual point .


Update: I add BoxSelect-module in this project at 03/18/2019 . Inporting FieldOfViewRenderer_andBoxSelect.unitypackage to see detail.

						Unity Version:2017.4.2f2  thank u very much !







