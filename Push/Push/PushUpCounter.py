import clr
clr.AddReference('Pythonnet')
import Pythonnet

Pythonnet.Runtime.PythonDLL = '/opt/homebrew/Cellar/python@3.9/3.9.6/Frameworks/Python.framework/Versions/3.9/lib/libpython3.9.dylib'  # Replace with the actual path

Pythonnet.PythonEngine.Initialize()
import cv2
import mediapipe as mp
import numpy as np
import PoseModule as pm


def run():
    cap = cv2.VideoCapture(0)
    detector = pm.poseDetector()
    count = 0
    direction = 0
    form = 0
    opacity = 0
    feedback = "Fix Form"
    prev_feedback = "Fix Form"

    # What needs to be fixed:
    # 1. Not robust to camera angles (need to figure out joint angles or body parts to track given different camera angles)
    # 2. The counting can easily be tricked (the 0.5 added and subtracted can be tricked if the person only goes down)
    # 3. Is this user friendly for a phone? Need to deploy it on a phone
    # 4. Need to figma up some of the UI. 
    # 5. May need instructions on how to use it
    #
    # What did I do:
    # 1. I removed some of the janky bar stuff
    # 2. I made it so it can count on both sides of the body and center (I removed the hip tracker)
    # 
    # !!!! Need to fix count !!!!
    #


    while cap.isOpened():
        ret, img = cap.read() #640 x 480
        #Determine dimensions of video - Help with creation of box in Line 43
        width  = cap.get(3)  # float `width`
        height = cap.get(4)  # float `height`
        # print(width, height)
        
        img = detector.findPose(img, False)
        lmList = detector.findPosition(img, False)
        # print(lmList)
        if len(lmList) != 0:
            left_elbow = detector.findAngle(img, 11, 13, 15)
            left_shoulder = detector.findAngle(img, 13, 11, 23)
            #left_hip = detector.findAngle(img, 11, 23, 25)
            
            right_elbow = detector.findAngle(img, 12, 14, 16)
            right_shoulder = detector.findAngle(img, 14, 12, 24)
            #right_hip = detector.findAngle(img, 12, 24, 26)
            
            #Percentage of success of pushup
            per_left = np.interp(left_elbow, (90, 160), (0, 100))
            per_right = np.interp(right_elbow, (90, 160), (0, 100))
            #Bar to show Pushup progress
            bar_left = np.interp(left_elbow, (90, 160), (380, 50))
            bar_right = np.interp(right_elbow, (90, 160), (380, 50))

            #Check to ensure right form before starting the program
            if (left_elbow > 160 and left_shoulder > 40) or (right_elbow > 160 and right_shoulder > 40):
                form = 1
        
            if form == 1:
                if per_left == 0 or per_right == 0:
                    if (left_elbow <= 90) or (right_elbow <= 90):
                        feedback = "Up"
                        if direction == 0:
                            direction = 1
                elif per_left == 100 or per_right == 100:
                    if (left_elbow > 160 and left_shoulder > 40) or (right_elbow > 160 and right_shoulder > 40):
                        feedback = "Down"
                        if direction == 1 and prev_feedback == "Up":
                            count += 1
                            direction = 0
                    
            prev_feedback = feedback 
        
            #print(count)
            # Update the opacity
            
            # Create a text with fading opacity
            text = str(int(count))
            font_scale = 8
            font_thickness = 5
            font_color = (255, 255, 255)
            text_size = cv2.getTextSize(text, cv2.FONT_HERSHEY_PLAIN, font_scale, font_thickness)[0]
            text_x = (img.shape[1] - text_size[0]) // 2
            text_y = (img.shape[0] + text_size[1]) // 2
            if feedback == "Down":
                cv2.putText(img, text, (text_x, text_y), cv2.FONT_HERSHEY_PLAIN, font_scale,
                            font_color, font_thickness)
            
            # Display the image
            cv2.imshow("Pushup Counter", img)
            
                
        if cv2.waitKey(10) & 0xFF == ord('q'):
            break
            
    cap.release()
    cv2.destroyAllWindows()