using System;
using System.Collections.Generic;
using UnityEngine;

public static class UndoRedoStack {

	static Stack<string> undoStack = new Stack<string>();
	static Stack<string> redoStack = new Stack<string>();
	public static string currentState;

	public static void Redo(){
		if(redoStack.Count == 0) {
			return;
		}
		undoStack.Push(currentState);
		currentState = redoStack.Pop();
		MapDataParser.ParseSaveString(currentState);
	}

	public static void Undo(){
		if(undoStack.Count == 0) {
			return;
		}
		redoStack.Push(currentState);
		currentState = undoStack.Pop();
		MapDataParser.ParseSaveString(currentState);

	}

	public static void newState(string data){
		redoStack.Clear();
		undoStack.Push(currentState);
		currentState = data;
	}
}

