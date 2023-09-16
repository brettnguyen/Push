import cv2
import mediapipe as mp
import numpy as np
import PoseModule as pm



cap = cv2.VideoCapture(0)
detector = pm.poseDetector()


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
        nose_x, nose_y = detector.findPoint(img, 0)
        
        # Display the image
        cv2.imshow("Plank", img)
        
            
    if cv2.waitKey(10) & 0xFF == ord('q'):
        break
        
cap.release()
cv2.destroyAllWindows()