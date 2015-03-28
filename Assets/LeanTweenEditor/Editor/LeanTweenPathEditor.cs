namespace DentedPixel
{
	namespace LTEditor
	{
		#region Using
		using UnityEditor;
		using UnityEngine;
		using System.Reflection;
		#endregion

		[CustomEditor (typeof(LeanTweenPath))]
		public class LeanTweenPathEditor : Editor {

			// Icon creation script
			const string iconString = "iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAAC4ElEQVQ4EUWT3WsUZxTGf+/M7G6yJmsSs9muxlTzUYJGEtGIYiGVNgjeae5KoUKgoPRfaOlF/4oWirYX3kVEpAhVlFZBUaOtiZouSUmMGJGQxKSbmd2Z6fPObirD4R3O53Oec45pKHwbG+OBxJhU8tb+t3T2dQFr11sXg0uMIy2hfow+R2+Y/BEbBdSkZjNEsayRI0lRjdKyumTSoRLE4f/OYegSVDxSaUinavpNP4Ojqs1NPvnWZXYXl+nZvcT+vpe4ToQXK4GtH0YuLbk1Phl+xMKbnTx42i2dx5nR3xkbvU8x/5ZC+wo7WjfJNUEmB5cuH3vfQuC7fDX2K+Njjzn7zXkqmw693Yt8d+4nPmiHd/+SdLb6zmF2oY2yn+PnKydtCxHVMKa9bZnjQ894/CLPw+mduKmAzo4lLl4ZZaq0l+EDJb7+/Cbf/3CaHyfOJIRGcUotiLgwyHB4X4nOgs+Fq/2sr3k0NpW5+6SPW/cHobKNT49OsrJmuH5nGD9waciIQA2lxoFTZeTQFH4F/njUg3ECiBBJAekGyO1Y48jAU/HyIXOLHTSkywkCDQanWjHsKrxlqH+ev+dbBDdPSvAtMjviSmD4qGueYnuZG/cO4osrqIoQa7dTkMORgVnybSETNwV/qRmyMtrlshxr9of2P2OjDHcm9+F5mwqO67tj1EY64NhgiQ2x3FVYZvyL60IgbMbRLsBv94YY7Cvx54sisy+FzvOV2KLQomnhvL49r+ntesP6Bko0w8jhGRuLK59IPDz/p0hP5yt+ufZZAj+brVgAye7oBvBOfTxN23b1KgIdBYYKcuRgq/81U6A5u0HztpC7k/1KauGruK1eF3PixEjclK1K7yiBdtKWT+Ab5l51aGSNInmV2w8PyEcH5dijsgdWew2NX4oR25MkMdj/mgOeTsxxicIMmcZIN1HX14Otv+eJRHuJtTPVeJRo62y3KpGKMLGWNun8PXwF8R9jRAcsz/JsQQAAAABJRU5ErkJggg==";
			static Texture2D icon;

			private enum PathCreateType{
				RectangleRounded,
				Snake,
				Random,
				Circle,
				Straight
			}

			private static int MAX_SPLINES = 64;

			private int didCopyIter;
			private PathCreateType selectedCreate = 0;
			private Vector3 pos;
			private int generateCode = 0;
			private int generateCodeLast = -1;
			private Vector2 scrollCodeViewPos;

			private float optionsSize = 5.0f;
			private float optionsLength = 2;
			private float optionsBevelSize = 1.0f;
			private float optionsRangeMin = 0.0f;
			private float optionsRangeMax = 1.0f;
			private int optionsSegments = 8;
			private Vector3 optionsDirection = new Vector3(1f,0f,0f);
			private string importArrValue;
			//private float optionsControlSize = 1.0f;

			public override void OnInspectorGUI()
			{
				LeanTweenPath path = target as LeanTweenPath;
				pos = path.transform.position;

				if( icon == null ){
					icon = new Texture2D(1,1);
					icon.hideFlags = HideFlags.HideAndDontSave;
					icon.LoadImage( System.Convert.FromBase64String( iconString ) );
				}
				Rect editorRect = EditorGUILayout.GetControlRect();
				GUI.DrawTexture( new Rect(16, editorRect.y - 16, icon.width, icon.height) , icon );

				EditorGUIUtility.labelWidth = 90f;

				LeanTweenPath.LeanTweenPathType pathTypeLast = path.pathType;
				path.pathType = (LeanTweenPath.LeanTweenPathType)EditorGUILayout.EnumPopup("Path Type", path.pathType);
				if(path.pathType!=pathTypeLast){
					resetPath();
				}
				EditorGUILayout.Separator();

				bool isBezier = path.pathType == LeanTweenPath.LeanTweenPathType.bezier;
				int k;

				EditorGUILayout.BeginHorizontal();
				GUI.color = LTEditor.shared.colorAddOn;
				path.creatorMaximized = EditorGUILayout.Foldout(path.creatorMaximized,"",LTEditor.shared.styleGroupFoldout);
				if(GUILayout.Button("Easy Path Creator", LTEditor.shared.styleGroupButton))
					path.creatorMaximized = !path.creatorMaximized;

				EditorGUILayout.EndHorizontal();

				if(path.creatorMaximized){
					selectedCreate = (PathCreateType)EditorGUILayout.EnumPopup("Type to create:", selectedCreate );

					optionsSize = EditorGUILayout.FloatField("Scale:", optionsSize);
					switch (selectedCreate) {
		                case PathCreateType.RectangleRounded:
		                    optionsBevelSize = EditorGUILayout.Slider("Bevel Size:", optionsBevelSize, 0.0f,1.399f);
		                    break;
		                case PathCreateType.Snake:
		                    optionsLength = EditorGUILayout.FloatField("Length:", optionsLength);
		                    break;
		                case PathCreateType.Random:
		                	optionsLength = EditorGUILayout.FloatField("Length:", optionsLength);
		                    EditorGUILayout.LabelField("Min Range:", optionsRangeMin.ToString());
				            EditorGUILayout.LabelField("Max Range:", optionsRangeMax.ToString());
				            EditorGUILayout.MinMaxSlider(ref optionsRangeMin, ref optionsRangeMax, 0.0f, 10.0f);

		                    break;
		                case PathCreateType.Circle:
		                    optionsSegments = closestSegment( EditorGUILayout.IntSlider("Segments:", optionsSegments, 4, MAX_SPLINES) );
		                    break;
		                case PathCreateType.Straight:
		                    optionsLength = EditorGUILayout.FloatField("Length:", optionsLength);
		                    optionsDirection = EditorGUILayout.Vector3Field("Direction:", optionsDirection);
		                    break;
		                default:
		                    break;
		            }


			        if(isBezier && GUILayout.Button("Create New Path",LTEditor.shared.styleActionButton)){
			        	
			        	// path.arraySize = count.intValue*4;
			        	switch (selectedCreate) {
			                case PathCreateType.RectangleRounded:
			                    resetPath();
			                    float b = optionsBevelSize;
			                    float n = 0.1f;
					           	addTo( 	new Vector3[]{ 
					           	 new Vector3(b,0f,0f),new Vector3(0f,0f,0f),new Vector3(0f,0f,0f),new Vector3(0f,b,0f)
					           	,new Vector3(0f,b,0f),new Vector3(0f,b+n,0f),new Vector3(0f,3f-b-n,0f),new Vector3(0f,3f-b,0f)
					           	,new Vector3(0f,3f-b,0f),new Vector3(0f,3f,0f),new Vector3(0f,3f,0f),new Vector3(b,3f,0f)
					           	,new Vector3(b,3f,0f),new Vector3(3f-b-n,3f,0f),new Vector3(b+n,3f,0f),new Vector3(3f-b,3f,0f)
					           	,new Vector3(3f-b,3f,0f),new Vector3(3f,3f,0f),new Vector3(3f,3f,0f),new Vector3(3f,3f-b,0f)
					           	,new Vector3(3f,3f-b,0f),new Vector3(3f,b+n,0f),new Vector3(3f,b+n,0f),new Vector3(3f,b,0f)
					           	,new Vector3(3f,b,0f),new Vector3(3f,0f,0f),new Vector3(3f,0f,0f),new Vector3(3f-b,0f,0f)
					           	,new Vector3(3f-b,0f,0f),new Vector3(3f-b-n,0f,0f),new Vector3(b+n,0f,0f),new Vector3(b,0f,0f)
					           	} );
			                    break;
			                case PathCreateType.Snake:
				                resetPath();
				                path.path = new Vector3[ (int)optionsLength*4 ];
			                    k = 0;
								for(int i=0; i < path.path.Length; i++){
						        	Vector3 vec3 = pos + new Vector3(k*optionsSize,0f,k*optionsSize);
						        	if(i%4==1){
						        		vec3.x += 1*optionsSize;
						        	}
						        	if(i%4==2){
						        		vec3.x += -1*optionsSize;
						        	}
						        	path.path[i] = vec3;
									if(i%2==0)
										k++;
						        }
			                    break;
			                case PathCreateType.Random:
				                resetPath();
			                	path.path = new Vector3[ (int)optionsLength*4 ];
			                    k = 0;
			                    float rand = 0f;
			                    for(int i=0; i < path.path.Length; i++){
						        	Vector3 vec3 = pos + new Vector3(k*optionsSize,0f,0f);
						        	
						        	if(i%4==1){
						        		vec3.z += rand*optionsSize;
						        	}
						        	rand = Random.Range(optionsRangeMin,optionsRangeMax)*optionsSize;
						        	if(i%4==2){
						        		vec3.z += -rand*optionsSize;
						        	}
						        	path.path[i] = vec3;
									if(i%2==0)
										k++;
						        }
			                    break;
			                case PathCreateType.Circle:
			                	resetPath();
			                	//count.intValue = optionsSegments;
			                    Vector3[] v = generateCircularQuadraticBezierSegments(optionsSize,optionsSegments);
			                    //Debug.Log("v:"+v);
			                    addTo(v);
			                    break;
			                case PathCreateType.Straight:
			                	resetPath();
			                	path.path = new Vector3[ (int)optionsLength*4 ];
			                	k = 0;
			                	for(int i=0; i < path.path.Length; i++){
						        	path.path[i] = pos + k*optionsDirection*optionsSize;
						        	if(i%2==0)
										k++;
						        }
			                    break;
			                default:
			                    Debug.LogError("Unrecognized Option");
			                    break;
			            }
			        }
				}

				EditorGUILayout.Separator();


				EditorGUILayout.BeginHorizontal();
				GUI.color = LTEditor.shared.colorGroupName;
				path.nodesMaximized = EditorGUILayout.Foldout(path.nodesMaximized,"",LTEditor.shared.styleGroupFoldout);
				if(GUILayout.Button("Nodes", LTEditor.shared.styleGroupButton))
					path.nodesMaximized = !path.nodesMaximized;

				GUI.color = LTEditor.shared.colorDelete;
				bool deleted = GUILayout.Button("Clear All", LTEditor.shared.styleClearAllButton);
				if(deleted)
					resetPath();
				EditorGUILayout.EndHorizontal();


				k = 0;
				int j = 0;
				for(int i = 0; i < path.pts.Length && path.nodesMaximized; i++){
					GUI.contentColor = LTEditor.shared.colorGroupName;
					Transform t = path.pts[i];
					GUI.color = Color.white;
					if(t==null){
						//EditorGUILayout.LabelField("Nothing here "+i);
					}else{
						EditorGUILayout.BeginHorizontal();
						
						string label;
						bool isPoint = true;
						if(isBezier){
							isPoint = i==0||i%4==3;
							if(isPoint){
								label = "  Node"+j;
								j++;
								k = 0;
							}else{
								label = "  - Control"+k;
								k++;
							}
						}else{
							label = "  Node"+i;
						}
						Vector3 position = EditorGUILayout.Vector3Field(label, t.localPosition);
						
						if (GUI.changed){
							Undo.RecordObject(t, "Transform Change "+t.gameObject.name);
				 			t.localPosition = FixIfNaN(position);
						}
						GUI.contentColor = Color.white;
						if(isPoint && i!=0){
							GUI.color = LTEditor.shared.colorDelete;
							if( GUILayout.Button("Delete", LTEditor.shared.styleDeleteButton) ){
								Undo.RecordObject(t, "Deleted Node "+t.gameObject.name);
								GameObject.DestroyImmediate(t.gameObject);
								// Debug.Log("before delete:"+path.count);
								path.count += isBezier ? -4 : -1;
								// Debug.Log("after:"+path.count);
								SceneView.RepaintAll();
							}
						}else{
							GUI.color = Color.white*0f;
							if( GUILayout.Button("Delete", LTEditor.shared.styleDeleteButton) ){
								Debug.Log("does nothing");
							}
						}

						EditorGUILayout.EndHorizontal();
					}
				}
				
				GUI.color = LTEditor.shared.colorAddTween;
				GUILayout.BeginHorizontal();

				GUILayout.FlexibleSpace();
				if(GUILayout.Button("+ Node", GUILayout.Width(60))){
					path.count += isBezier ? 4 : 1;
					SceneView.RepaintAll();
				}
				GUILayout.EndHorizontal();

				EditorGUILayout.Separator();
				GUI.color = Color.white;

				if(GUILayout.Button("Reverse Path Direction")){
		        	Vector3[] newPath = new Vector3[ path.pts.Length ];
		        	for(int i=0; i < newPath.Length; i++){
		        		// Debug.Log("pt at:"+(newPath.Length-i-1)+ " len:"+newPath.Length);
		        		Transform trans = path.pts[newPath.Length-i-1];
		        		if(trans==null)
		        			trans = path.pts[newPath.Length-i-2];
		        		newPath[i] = trans.position;
		        	}

		        	for(int i=0; i < path.pts.Length; i++){
		        		if(path.pts[i]){
				        	path.pts[i].position = newPath[i];
						}
			        }
		        }

		        float controlSizeBefore = path.controlSize;
		        path.controlSize = EditorGUILayout.FloatField("Control Size", path.controlSize);
		        if(path.controlSize!=controlSizeBefore)
		        	SceneView.RepaintAll();
		        	
		        // EditorGUILayout.LabelField("Count: "+path.count );
				
		        EditorGUILayout.Separator();
		        EditorGUILayout.Separator();
			    if(isBezier){
			    	EditorGUILayout.LabelField("Import/Export", EditorStyles.boldLabel );
				    EditorGUILayout.BeginHorizontal(GUILayout.Width(200));
				    EditorGUILayout.LabelField("Code Type", GUILayout.Width(80) );
				    generateCode = EditorGUILayout.Popup( generateCode, new string[]{"C#","Unityscript"},GUILayout.Width(80));
				    EditorGUILayout.EndHorizontal();

				    if(generateCode!=generateCodeLast){
				    	string codeFormat = generateCode==0 ? "new Vector3[]{ new Vector3(0f,0f,0f), ... }" : "[ Vector3(0.0,0.0,0.0), ... ]";
				    	importArrValue = "Paste in format: "+codeFormat;
				    	generateCodeLast = generateCode;
				    }

					EditorGUILayout.LabelField("Export:");
					if(Application.isPlaying==false){
						string outputStr;
						if(generateCode==0){
							outputStr = "new Vector3[]{ ";
					        for(int i=0; i < path.pts.Length; i++){
								Transform trans = i>3 && i%4==0 ? path.pts[i-1] : path.pts[i];
								if(trans!=null){
									outputStr += "new Vector3("+trans.localPosition.x+"f,"+trans.localPosition.y+"f,"+trans.localPosition.z+"f)";
									if(i<path.pts.Length-1)
										outputStr += ", ";
								}
							}
							outputStr += " }";
				        }else{
				        	outputStr = "[";
					        for(int i=0; i < path.pts.Length; i++){
								Transform trans = i>3 && i%4==0 ? path.pts[i-1] : path.pts[i];
								if(trans!=null){
									outputStr += "Vector3("+trans.localPosition.x+","+trans.localPosition.y+","+trans.localPosition.z+")";
									if(i<path.pts.Length-1)
										outputStr += ", ";
								}
							}
							outputStr += "]";
				        }

						scrollCodeViewPos = EditorGUILayout.BeginScrollView(scrollCodeViewPos, GUILayout.Height(70) );	
					
						EditorGUILayout.TextArea( outputStr, LTEditor.shared.styleCodeTextArea );	
					
						EditorGUILayout.EndScrollView();

					}else{
						EditorGUILayout.LabelField("    Not available during runtime");
					}
					
					EditorGUILayout.LabelField("Import:");
					
					importArrValue = EditorGUILayout.TextArea( importArrValue, LTEditor.shared.styleCodeTextArea );
					
					GUILayout.BeginHorizontal();
					GUILayout.FlexibleSpace();
					if(GUILayout.Button(" Import Path ")){
						// Debug.Log("index:"+importArrValue.IndexOf("Paste")+" val:"+importArrValue);
						if(importArrValue.IndexOf("Paste") < 0){
							// Debug.Log("Importing");
							string subImport = "";
							if(generateCode==0){
								int start = importArrValue.IndexOf("{");
								int end = importArrValue.IndexOf("}");
								if(start<0){
									Debug.LogError("No starting bracket'{'");
								}else if(end<0){
									Debug.LogError("No ending bracket'}'");
								}else{
									subImport = importArrValue.Substring(start+1,end-1-start).Trim();
									// Debug.Log("broken down importedArr:"+subImport);
								}
							}else{
								int start = importArrValue.IndexOf("[");
								int end = importArrValue.IndexOf("]");
								if(start<0){
									Debug.LogError("No starting bracket'['");
								}else if(end<0){
									Debug.LogError("No ending bracket']'");
								}else{
									subImport = importArrValue.Substring(start+1,end-1-start).Trim();
									// Debug.Log("broken down importedArr:"+subImport);
								}
							}

							string[] stringArr = subImport.Split(new string[]{"Vector3"}, System.StringSplitOptions.RemoveEmptyEntries);
							int len = generateCode==0 ? (stringArr.Length-1) : stringArr.Length;
							// Debug.Log("importing length:"+len);
							resetPath();
							path.path = new Vector3[ len ];
							k = 0;
							for(int i = 0; i < stringArr.Length; i++){
								int start = stringArr[i].IndexOf("("[0]);
								int end = stringArr[i].IndexOf(")"[0]);
								// Debug.Log("i:"+i+" start:"+start+" end:"+end);
								if(start>=0 && end>0){
									string vecStr = stringArr[i].Substring(start+1,end-1-start);
									// Debug.Log("vec:"+vecStr);
									string[] numArr = vecStr.Split(","[0]);
									for(j = 0; j < numArr.Length; j++){
										if(numArr[j].IndexOf("("[0])>=0){
											start = numArr[j].IndexOf("("[0]);
											numArr[j] = numArr[j].Substring(start+1);
										}else if(numArr[j].IndexOf(")"[0])>=0){
											end = numArr[j].IndexOf(")"[0]);
											numArr[j] = numArr[j].Substring(0,end);
										}
										if(numArr[j].IndexOf("f"[0])>=0){
											end = numArr[j].IndexOf("f"[0]);
											numArr[j] = numArr[j].Substring(0,end);
										}
										// Debug.Log("num:"+numArr[j]);
									}

									Vector3 vec3 = new Vector3(float.Parse(numArr[0]),float.Parse(numArr[1]),float.Parse(numArr[2]));
									path.path[k] = vec3;
									// Debug.Log("importing vec3:"+vec3);
									k++;
								}
							}
						}
					}
					GUILayout.EndHorizontal();
				}

			}

			private void addTo( Vector3[] add ){
				LeanTweenPath path = target as LeanTweenPath;

				path.path = new Vector3[ add.Length ];
				for(int i =0;i<add.Length;i++){
					path.path[i] = pos + add[i] * optionsSize;
				}
			}

			void resetPath(){
				// Debug.Log("resetting path...");
				LeanTweenPath path = target as LeanTweenPath;

				path.pts = new Transform[0]; // set to zero to reset
				for (int i=path.gameObject.transform.childCount-1; i>=0; i--) { // Destroy anything currently a child
					DestroyImmediate( path.gameObject.transform.GetChild(i).gameObject );
				}
				SceneView.RepaintAll();
			}

			private int closestSegment( int val ){
				if(val<6)
					return 4;
				if(val < 12)
					return 8;
				if(val < 24)
					return 16;
				if(val < 48)
					return 32;

				return 64;
			}

			private static Vector3 FixIfNaN(Vector3 v)
			{
				if (float.IsNaN(v.x))
					v.x = 0;
				if (float.IsNaN(v.y))
					v.y = 0;
				if (float.IsNaN(v.z))
					v.z = 0;
				return v;
			}

			private static float[] fudgeMulti = new float[]{0f,0f,0f,0.16f,0f,0f,0f,0.04f,0f,0f,0f,0.02f,0f,0f,0f,0.01f,0f,0f,0f,0.2f,0f,0f,0f,0.06f,0f,0f,0f,0.02f,0f,0f,0f,0.002f,0f,0f,0f,0.2f,0.2f,0.2f,0.2f,0.06f,0.06f,0.06f,0.06f,0.02f,0.06f,0.06f,0.06f,0.02f,0f,0f,0f,0.2f,0.2f,0.2f,0.2f,0.06f,0.06f,0.06f,0.06f,0.02f,0.06f,0.06f,0.06f,0.0003f};

			public static Vector3[] generateCircularQuadraticBezierSegments(float radius, int numControlPoints)
		    {
		        Vector3[] segments = new Vector3[ numControlPoints * 4 ];
		        float arcLength = 2 * Mathf.PI / numControlPoints;
		        float controlRadius;
		 		
		 		float fudge = 0.14f;
		 		// Debug.Log("arcLength:"+arcLength + " fudge:"+fudge);
		 		float controlMult = 1 - fudgeMulti[ numControlPoints - 1];// - 1.0 / numControlPoints * 0.5;

		        for (int i = 0; i < numControlPoints; i++) {

		            float startX = radius * Mathf.Cos(arcLength * i);
		            float startY = radius * Mathf.Sin(arcLength * i);
		            segments[i*4+0] = new Vector3(startX, startY, 0f);

		            //control radius formula
		            //where does it come from, why does it work?
		            controlRadius = radius / Mathf.Cos(arcLength * .5f);

		            //the control point is plotted halfway between the arcLength and uses the control radius
		            float controlX = controlRadius * Mathf.Cos(arcLength * (i + 1 + fudge) - arcLength * .5f) * controlMult;
		            float controlY = controlRadius * Mathf.Sin(arcLength * (i + 1 + fudge) - arcLength * .5f) * controlMult;
		            segments[i*4+1] = new Vector3(controlX, controlY, 0f);

		            controlX = controlRadius * Mathf.Cos(arcLength * (i + 1 - fudge) - arcLength * .5f) * controlMult;
		            controlY = controlRadius * Mathf.Sin(arcLength * (i + 1 - fudge) - arcLength * .5f) * controlMult;
		            segments[i*4+2] = new Vector3(controlX, controlY, 0f);

		            float endX = radius * Mathf.Cos(arcLength * (i + 1));
		            float endY = radius * Mathf.Sin(arcLength * (i + 1));
		            segments[i*4+3] = new Vector3(endX, endY, 0f);
		        }

		        return segments;
		    }

			[MenuItem ("GameObject/Create Other/LeanTweenPath")]
			static void CreateLeanTweenPath()
			{
				GameObject go = new GameObject("LeanTweenPath");
				go.AddComponent<LeanTweenPath>();
			}
			
		}

		

	}// end LTEditor
}// end Dented Pixel
