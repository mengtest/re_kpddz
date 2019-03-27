using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_CardAnimation : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int slot2Position(IntPtr l) {
		try {
			CardAnimation self=(CardAnimation)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			var ret=self.slot2Position(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int press(IntPtr l) {
		try {
			CardAnimation self=(CardAnimation)checkSelf(l);
			System.Boolean a1;
			checkType(l,2,out a1);
			self.press(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int drag(IntPtr l) {
		try {
			CardAnimation self=(CardAnimation)checkSelf(l);
			UnityEngine.Vector2 a1;
			checkType(l,2,out a1);
			self.drag(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int init(IntPtr l) {
		try {
			CardAnimation self=(CardAnimation)checkSelf(l);
			self.init();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int selectLeft(IntPtr l) {
		try {
			CardAnimation self=(CardAnimation)checkSelf(l);
			self.selectLeft();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int selectRight(IntPtr l) {
		try {
			CardAnimation self=(CardAnimation)checkSelf(l);
			self.selectRight();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int OnDestroy(IntPtr l) {
		try {
			CardAnimation self=(CardAnimation)checkSelf(l);
			self.OnDestroy();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_MOVE_TIME(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,CardAnimation.MOVE_TIME);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_MOVE_TIME(IntPtr l) {
		try {
			System.Single v;
			checkType(l,2,out v);
			CardAnimation.MOVE_TIME=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_cardSize(IntPtr l) {
		try {
			CardAnimation self=(CardAnimation)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.cardSize);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_cardSize(IntPtr l) {
		try {
			CardAnimation self=(CardAnimation)checkSelf(l);
			UnityEngine.Vector2 v;
			checkType(l,2,out v);
			self.cardSize=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_interval(IntPtr l) {
		try {
			CardAnimation self=(CardAnimation)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.interval);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_interval(IntPtr l) {
		try {
			CardAnimation self=(CardAnimation)checkSelf(l);
			System.Int32 v;
			checkType(l,2,out v);
			self.interval=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_scaleCurve(IntPtr l) {
		try {
			CardAnimation self=(CardAnimation)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.scaleCurve);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_scaleCurve(IntPtr l) {
		try {
			CardAnimation self=(CardAnimation)checkSelf(l);
			UnityEngine.AnimationCurve v;
			checkType(l,2,out v);
			self.scaleCurve=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_posCurve(IntPtr l) {
		try {
			CardAnimation self=(CardAnimation)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.posCurve);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_posCurve(IntPtr l) {
		try {
			CardAnimation self=(CardAnimation)checkSelf(l);
			UnityEngine.AnimationCurve v;
			checkType(l,2,out v);
			self.posCurve=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_cardCount(IntPtr l) {
		try {
			CardAnimation self=(CardAnimation)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.cardCount);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_cardCount(IntPtr l) {
		try {
			CardAnimation self=(CardAnimation)checkSelf(l);
			System.Int32 v;
			checkType(l,2,out v);
			self.cardCount=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_seqPriority(IntPtr l) {
		try {
			CardAnimation self=(CardAnimation)checkSelf(l);
			pushValue(l,true);
			pushEnum(l,(int)self.seqPriority);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_seqPriority(IntPtr l) {
		try {
			CardAnimation self=(CardAnimation)checkSelf(l);
			CardAnimation.ESequencePriority v;
			checkEnum(l,2,out v);
			self.seqPriority=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_posOffset(IntPtr l) {
		try {
			CardAnimation self=(CardAnimation)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.posOffset);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_posOffset(IntPtr l) {
		try {
			CardAnimation self=(CardAnimation)checkSelf(l);
			System.Single v;
			checkType(l,2,out v);
			self.posOffset=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_centerExtraRange(IntPtr l) {
		try {
			CardAnimation self=(CardAnimation)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.centerExtraRange);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_centerExtraRange(IntPtr l) {
		try {
			CardAnimation self=(CardAnimation)checkSelf(l);
			System.Single v;
			checkType(l,2,out v);
			self.centerExtraRange=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_cycle(IntPtr l) {
		try {
			CardAnimation self=(CardAnimation)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.cycle);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_cycle(IntPtr l) {
		try {
			CardAnimation self=(CardAnimation)checkSelf(l);
			System.Boolean v;
			checkType(l,2,out v);
			self.cycle=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_changeScale(IntPtr l) {
		try {
			CardAnimation self=(CardAnimation)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.changeScale);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_changeScale(IntPtr l) {
		try {
			CardAnimation self=(CardAnimation)checkSelf(l);
			System.Boolean v;
			checkType(l,2,out v);
			self.changeScale=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_onStartEnterForegroundEvent(IntPtr l) {
		try {
			CardAnimation self=(CardAnimation)checkSelf(l);
			CardAnimation.onCardStartEnterForeground v;
			int op=LuaDelegation.checkDelegate(l,2,out v);
			if(op==0) self.onStartEnterForegroundEvent=v;
			else if(op==1) self.onStartEnterForegroundEvent+=v;
			else if(op==2) self.onStartEnterForegroundEvent-=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_onCardStartEnterBackgroundEvent(IntPtr l) {
		try {
			CardAnimation self=(CardAnimation)checkSelf(l);
			CardAnimation.onCardStartEnterBackground v;
			int op=LuaDelegation.checkDelegate(l,2,out v);
			if(op==0) self.onCardStartEnterBackgroundEvent=v;
			else if(op==1) self.onCardStartEnterBackgroundEvent+=v;
			else if(op==2) self.onCardStartEnterBackgroundEvent-=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_onCardAxisSideStartChangeEvent(IntPtr l) {
		try {
			CardAnimation self=(CardAnimation)checkSelf(l);
			CardAnimation.onCardAxisSideStartChange v;
			int op=LuaDelegation.checkDelegate(l,2,out v);
			if(op==0) self.onCardAxisSideStartChangeEvent=v;
			else if(op==1) self.onCardAxisSideStartChangeEvent+=v;
			else if(op==2) self.onCardAxisSideStartChangeEvent-=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_onCardStopEvent(IntPtr l) {
		try {
			CardAnimation self=(CardAnimation)checkSelf(l);
			CardAnimation.onCardStop v;
			int op=LuaDelegation.checkDelegate(l,2,out v);
			if(op==0) self.onCardStopEvent=v;
			else if(op==1) self.onCardStopEvent+=v;
			else if(op==2) self.onCardStopEvent-=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_leftUpperBound(IntPtr l) {
		try {
			CardAnimation self=(CardAnimation)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.leftUpperBound);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_rightUpperBound(IntPtr l) {
		try {
			CardAnimation self=(CardAnimation)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.rightUpperBound);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_direction(IntPtr l) {
		try {
			CardAnimation self=(CardAnimation)checkSelf(l);
			pushValue(l,true);
			pushEnum(l,(int)self.direction);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_activeIndex(IntPtr l) {
		try {
			CardAnimation self=(CardAnimation)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.activeIndex);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_onCardUpdateEvent(IntPtr l) {
		try {
			CardAnimation self=(CardAnimation)checkSelf(l);
			CardAnimation.onCardUpdate v;
			int op=LuaDelegation.checkDelegate(l,2,out v);
			if(op==0) self.onCardUpdateEvent=v;
			else if(op==1) self.onCardUpdateEvent+=v;
			else if(op==2) self.onCardUpdateEvent-=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"CardAnimation");
		addMember(l,slot2Position);
		addMember(l,press);
		addMember(l,drag);
		addMember(l,init);
		addMember(l,selectLeft);
		addMember(l,selectRight);
		addMember(l,OnDestroy);
		addMember(l,"MOVE_TIME",get_MOVE_TIME,set_MOVE_TIME,false);
		addMember(l,"cardSize",get_cardSize,set_cardSize,true);
		addMember(l,"interval",get_interval,set_interval,true);
		addMember(l,"scaleCurve",get_scaleCurve,set_scaleCurve,true);
		addMember(l,"posCurve",get_posCurve,set_posCurve,true);
		addMember(l,"cardCount",get_cardCount,set_cardCount,true);
		addMember(l,"seqPriority",get_seqPriority,set_seqPriority,true);
		addMember(l,"posOffset",get_posOffset,set_posOffset,true);
		addMember(l,"centerExtraRange",get_centerExtraRange,set_centerExtraRange,true);
		addMember(l,"cycle",get_cycle,set_cycle,true);
		addMember(l,"changeScale",get_changeScale,set_changeScale,true);
		addMember(l,"onStartEnterForegroundEvent",null,set_onStartEnterForegroundEvent,true);
		addMember(l,"onCardStartEnterBackgroundEvent",null,set_onCardStartEnterBackgroundEvent,true);
		addMember(l,"onCardAxisSideStartChangeEvent",null,set_onCardAxisSideStartChangeEvent,true);
		addMember(l,"onCardStopEvent",null,set_onCardStopEvent,true);
		addMember(l,"leftUpperBound",get_leftUpperBound,null,true);
		addMember(l,"rightUpperBound",get_rightUpperBound,null,true);
		addMember(l,"direction",get_direction,null,true);
		addMember(l,"activeIndex",get_activeIndex,null,true);
		addMember(l,"onCardUpdateEvent",null,set_onCardUpdateEvent,true);
		createTypeMetatable(l,null, typeof(CardAnimation),typeof(UnityEngine.MonoBehaviour));
	}
}
